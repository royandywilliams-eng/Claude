const express = require('express');
const fs = require('fs');
const path = require('path');

const app = express();
const PORT = 3000;

// Middleware
app.use(express.static(__dirname));
app.use(express.json());

// Load NT data
let ntData = {};
try {
  const dataPath = path.join(__dirname, 'data', 'nt-data.json');
  ntData = JSON.parse(fs.readFileSync(dataPath, 'utf8'));
} catch (error) {
  console.error('Error loading NT data:', error);
}

// Routes
app.get('/', (req, res) => {
  res.sendFile(path.join(__dirname, 'index.html'));
});

// Get all books
app.get('/api/books', (req, res) => {
  const books = ntData.books.map(b => ({ id: b.id, name: b.name }));
  res.json(books);
});

// Get questions for a specific book
app.get('/api/book/:id', (req, res) => {
  const bookId = parseInt(req.params.id);
  const book = ntData.books.find(b => b.id === bookId);
  if (book) {
    res.json(book);
  } else {
    res.status(404).json({ error: 'Book not found' });
  }
});

// Get chapters for a specific book with Q&A counts
app.get('/api/book/:id/chapters', (req, res) => {
  const bookId = parseInt(req.params.id);
  const book = ntData.books.find(b => b.id === bookId);

  if (!book) {
    return res.status(404).json({ error: 'Book not found' });
  }

  // Group Q&A by chapter
  const chapterMap = {};
  book.qa.forEach(item => {
    const chapter = item.chapter;
    if (!chapterMap[chapter]) {
      chapterMap[chapter] = 0;
    }
    chapterMap[chapter]++;
  });

  const chapters = Object.keys(chapterMap)
    .sort((a, b) => {
      if (a === 'null') return -1;
      if (b === 'null') return 1;
      return parseInt(a) - parseInt(b);
    })
    .map(ch => ({
      chapter: ch === 'null' ? null : parseInt(ch),
      count: chapterMap[ch]
    }));

  res.json({
    id: book.id,
    name: book.name,
    chapters: chapters
  });
});

// Get Q&A for a specific chapter in a book
app.get('/api/book/:id/chapter/:chapterNum', (req, res) => {
  const bookId = parseInt(req.params.id);
  const chapterNum = req.params.chapterNum === 'null' ? null : parseInt(req.params.chapterNum);

  const book = ntData.books.find(b => b.id === bookId);
  if (!book) {
    return res.status(404).json({ error: 'Book not found' });
  }

  const filteredQA = book.qa.filter(item => item.chapter === chapterNum);

  res.json({
    id: book.id,
    name: book.name,
    chapter: chapterNum,
    qa: filteredQA
  });
});

// Search questions and answers
app.get('/api/search', (req, res) => {
  const query = req.query.q.toLowerCase();
  const results = [];

  ntData.books.forEach(book => {
    book.qa.forEach((item, qaIndex) => {
      if (item.q.toLowerCase().includes(query) || item.a.toLowerCase().includes(query)) {
        results.push({
          book: book.name,
          question: item.q,
          answer: item.a,
          bookId: book.id,
          qaIndex: qaIndex
        });
      }
    });
  });

  res.json(results);
});

// Get random question
app.get('/api/random', (req, res) => {
  const randomBook = ntData.books[Math.floor(Math.random() * ntData.books.length)];
  const qaIndex = Math.floor(Math.random() * randomBook.qa.length);
  const randomQA = randomBook.qa[qaIndex];

  res.json({
    book: randomBook.name,
    question: randomQA.q,
    answer: randomQA.a,
    bookId: randomBook.id,
    qaIndex: qaIndex
  });
});

// Export all Q&A as text file
app.get('/api/export', (req, res) => {
  let exportText = 'New Testament Q&A Export\n';
  exportText += '============================\n\n';

  ntData.books.forEach(book => {
    exportText += `=== ${book.name} (${book.chapters} chapters) ===\n`;
    book.qa.forEach(item => {
      exportText += `Q: ${item.q}\n`;
      exportText += `A: ${item.a}\n\n`;
    });
    exportText += '\n';
  });

  res.type('text/plain');
  res.send(exportText);
});

// Export selected Q&A as text file
app.post('/api/export-selected', (req, res) => {
  const { selected } = req.body;

  if (!selected || !Array.isArray(selected) || selected.length === 0) {
    return res.status(400).json({ error: 'No items selected' });
  }

  let exportText = 'New Testament Q&A - Selected Export\n';
  exportText += '====================================\n\n';

  // Group selected items by book for organized output
  const itemsByBook = {};

  selected.forEach(({ bookId, qaIndex }) => {
    const book = ntData.books.find(b => b.id === bookId);
    if (book && book.qa[qaIndex]) {
      if (!itemsByBook[bookId]) {
        itemsByBook[bookId] = {
          name: book.name,
          chapters: book.chapters,
          items: []
        };
      }
      itemsByBook[bookId].items.push(book.qa[qaIndex]);
    }
  });

  // Format output by book
  Object.values(itemsByBook).forEach(book => {
    exportText += `=== ${book.name} (${book.chapters} chapters) ===\n`;
    book.items.forEach(qa => {
      exportText += `Q: ${qa.q}\n`;
      exportText += `A: ${qa.a}\n\n`;
    });
    exportText += '\n';
  });

  res.type('text/plain');
  res.send(exportText);
});

// Summarization helper functions
function extractKeywords(text) {
  // Remove common words and extract meaningful terms
  const commonWords = new Set(['the', 'a', 'an', 'and', 'or', 'but', 'in', 'of', 'to', 'is', 'are', 'was', 'were', 'be', 'been', 'being', 'by', 'at', 'for', 'on', 'with', 'from', 'as', 'it', 'that', 'this', 'which', 'who', 'whom', 'what', 'when', 'where', 'why', 'how', 'all', 'each', 'every', 'both', 'few', 'more', 'most', 'other', 'some', 'such', 'no', 'not', 'only', 'own', 'same', 'so', 'than', 'too', 'very']);

  const words = text.toLowerCase()
    .split(/[\s\.,;:!?—-]+/)
    .filter(w => w.length > 3 && !commonWords.has(w));

  // Count word frequency
  const freq = {};
  words.forEach(w => {
    freq[w] = (freq[w] || 0) + 1;
  });

  // Get top keywords
  return Object.entries(freq)
    .sort((a, b) => b[1] - a[1])
    .slice(0, 8)
    .map(([word]) => word)
    .join(', ');
}

function generateExtractiveSummary(qaItems) {
  if (qaItems.length === 0) return 'No content to summarize.';

  const summary = qaItems
    .map((item, idx) => {
      const keywords = extractKeywords(item.q + ' ' + item.a);
      return `${idx + 1}. ${item.q}\n   Key concepts: ${keywords}`;
    })
    .join('\n\n');

  return `Summary of ${qaItems.length} Q&A items:\n\n${summary}`;
}

async function generateAISummary(qaItems, apiKey) {
  // Validate API key format
  if (!apiKey || !apiKey.startsWith('sk-')) {
    throw new Error('Invalid OpenAI API key format');
  }

  const content = qaItems
    .map((item, idx) => `${idx + 1}. Q: ${item.q}\nA: ${item.a}`)
    .join('\n\n');

  const prompt = `Please provide a concise, comprehensive summary of the following Q&A content from the New Testament. Focus on key themes, concepts, and teachings. Keep the summary to 2-3 paragraphs:\n\n${content}`;

  try {
    const response = await fetch('https://api.openai.com/v1/chat/completions', {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${apiKey}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        model: 'gpt-3.5-turbo',
        messages: [
          { role: 'user', content: prompt }
        ],
        max_tokens: 500
      })
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.error?.message || 'OpenAI API error');
    }

    const data = await response.json();
    return data.choices[0].message.content;
  } catch (error) {
    throw new Error(`AI summarization failed: ${error.message}`);
  }
}

// Summarization endpoint
app.post('/api/summarize', async (req, res) => {
  const { selected, mode, apiKey } = req.body;

  if (!selected || !Array.isArray(selected) || selected.length === 0) {
    return res.status(400).json({ error: 'No items selected' });
  }

  if (!mode || !['ai', 'extractive'].includes(mode)) {
    return res.status(400).json({ error: 'Invalid summarization mode' });
  }

  try {
    // Collect Q&A items
    const qaItems = [];
    selected.forEach(({ bookId, qaIndex }) => {
      const book = ntData.books.find(b => b.id === bookId);
      if (book && book.qa[qaIndex]) {
        qaItems.push(book.qa[qaIndex]);
      }
    });

    if (qaItems.length === 0) {
      return res.status(400).json({ error: 'No valid items found' });
    }

    let summary;
    if (mode === 'extractive') {
      summary = generateExtractiveSummary(qaItems);
    } else {
      summary = await generateAISummary(qaItems, apiKey);
    }

    res.json({
      summary,
      itemCount: qaItems.length,
      wordCount: summary.split(/\s+/).length,
      mode
    });
  } catch (error) {
    console.error('Summarization error:', error);
    res.status(500).json({ error: error.message });
  }
});

// Start server
app.listen(PORT, () => {
  console.log(`NT Q&A App running on http://localhost:${PORT}`);
  console.log('Press Ctrl+C to stop the server');
});
