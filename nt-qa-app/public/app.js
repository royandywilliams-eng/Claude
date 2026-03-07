// ============= SELECTION STATE MANAGEMENT =============
const selectedItems = new Set();

function addItemToSelection(bookId, qaIndex) {
    const itemId = `${bookId}:${qaIndex}`;
    selectedItems.add(itemId);
    updateSelectionUI();
}

function removeItemFromSelection(bookId, qaIndex) {
    const itemId = `${bookId}:${qaIndex}`;
    selectedItems.delete(itemId);
    updateSelectionUI();
}

function toggleItemSelection(bookId, qaIndex) {
    const itemId = `${bookId}:${qaIndex}`;
    if (selectedItems.has(itemId)) {
        selectedItems.delete(itemId);
    } else {
        selectedItems.add(itemId);
    }
    updateSelectionUI();
}

async function updateSelectionUI() {
    const count = selectedItems.size;
    const banner = document.getElementById('selection-banner');
    const counter = document.getElementById('selection-counter');
    const selectedCountEl = document.getElementById('selected-count');
    const exportSelectedBtn = document.getElementById('export-selected-btn');
    const selectionListContainer = document.getElementById('selection-list-container');
    const selectionList = document.getElementById('selection-list');

    // Update counter text
    counter.textContent = `📌 ${count} ${count === 1 ? 'item' : 'items'} selected`;

    // Update banner visibility
    if (count > 0) {
        banner.classList.remove('hidden');
    } else {
        banner.classList.add('hidden');
    }

    // Update export tab info
    if (count > 0) {
        selectedCountEl.textContent = `${count} ${count === 1 ? 'item' : 'items'} selected`;
        exportSelectedBtn.disabled = false;
        document.getElementById('summarize-btn').disabled = false;
        selectionListContainer.classList.remove('hidden');

        // Update selection list
        const selected = getSelectedItemsData();
        let listHTML = '';

        for (const item of selected) {
            const book = await getBookNameById(item.bookId);
            if (book) {
                // Get the question preview (limit to 50 chars)
                const question = getQuestionPreview(item.bookId, item.qaIndex, 50);
                listHTML += `
                    <div class="selection-item">
                        <span class="selection-item-text">${book} - Q${item.qaIndex + 1}</span>
                    </div>
                `;
            }
        }

        selectionList.innerHTML = listHTML;
    } else {
        selectedCountEl.textContent = 'No items selected yet';
        exportSelectedBtn.disabled = true;
        document.getElementById('summarize-btn').disabled = true;
        selectionListContainer.classList.add('hidden');
    }

    // Highlight selected items in DOM
    document.querySelectorAll('.qa-item').forEach(item => {
        const addBtn = item.querySelector('.btn-add-item');
        if (addBtn) {
            const bookId = addBtn.dataset.bookId;
            const qaIndex = addBtn.dataset.qaIndex;
            const itemId = `${bookId}:${qaIndex}`;

            if (selectedItems.has(itemId)) {
                item.classList.add('selected');
                addBtn.textContent = '✓ Added';
            } else {
                item.classList.remove('selected');
                addBtn.textContent = '➕ Add';
            }
        }
    });
}

// Store books for quick lookup
let booksCache = null;

async function getBooksCache() {
    if (!booksCache) {
        const response = await fetch('/api/books');
        const books = await response.json();
        booksCache = books;
    }
    return booksCache;
}

async function getBookNameById(bookId) {
    const books = await getBooksCache();
    const book = books.find(b => b.id === bookId);
    return book ? book.name : null;
}

function getQuestionPreview(bookId, qaIndex, maxLength) {
    // This is a placeholder - in real implementation, we'd fetch the actual question
    return `Item ${qaIndex + 1}`;
}

function clearSelection() {
    selectedItems.clear();
    updateSelectionUI();
}

// ============= CHAPTER LOADING & FILTERING =============
async function loadChapters(bookId) {
    try {
        const response = await fetch(`/api/book/${bookId}/chapters`);
        const data = await response.json();

        const chapterSelect = document.getElementById('chapter-select');
        chapterSelect.innerHTML = '<option value="">View entire book</option>';

        // Add chapter options
        if (data.chapters) {
            data.chapters.forEach(ch => {
                const option = document.createElement('option');
                option.value = ch.chapter === null ? 'null' : ch.chapter;
                const chapterLabel = ch.chapter === null ? 'Book-wide' : `Chapter ${ch.chapter}`;
                option.textContent = `${chapterLabel} (${ch.count} items)`;
                chapterSelect.appendChild(option);
            });
        }

        document.getElementById('chapter-selector').classList.remove('hidden');
    } catch (error) {
        console.error('Error loading chapters:', error);
    }
}

async function loadChapterQA(bookId, chapter) {
    try {
        const url = chapter ? `/api/book/${bookId}/chapter/${chapter}` : `/api/book/${bookId}`;
        const response = await fetch(url);
        const book = await response.json();

        const container = document.getElementById('book-qa');
        container.innerHTML = book.qa.map((qa, qaIndex) => `
            <div class="qa-item" data-chapter="${qa.chapter || 'N/A'}">
                <div class="qa-chapter">Chapter ${qa.chapter || 'Book-wide'}</div>
                <div class="qa-book">${book.name}</div>
                <div class="qa-question">${qa.q}</div>
                <div class="qa-answer">${qa.a}</div>
                <button class="btn-add-item" data-book-id="${book.id}" data-qa-index="${qaIndex}">➕ Add</button>
            </div>
        `).join('');

        updateSelectionUI();
    } catch (error) {
        console.error('Error loading chapter Q&A:', error);
        const container = document.getElementById('book-qa');
        container.innerHTML = '<div class="empty-state"><p>Error loading content</p></div>';
    }
}

// ============= SUMMARIZATION FUNCTIONS =============
async function generateSummary() {
    if (selectedItems.size === 0) {
        alert('Please select at least one item to summarize');
        return;
    }

    const mode = document.querySelector('input[name="summary-mode"]:checked').value;
    const selected = getSelectedItemsData();

    const payload = { selected, mode };

    if (mode === 'ai') {
        const apiKey = document.getElementById('openai-key').value.trim();
        if (!apiKey) {
            alert('Please enter your OpenAI API key for AI summarization');
            return;
        }
        payload.apiKey = apiKey;
    }

    try {
        document.getElementById('summarize-btn').disabled = true;
        document.getElementById('summarize-btn').textContent = '⏳ Generating...';

        const response = await fetch('/api/summarize', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.error || 'Summarization failed');
        }

        const data = await response.json();
        showSummaryResult(data.summary, mode, data.itemCount, data.wordCount);

    } catch (error) {
        console.error('Summarization error:', error);
        alert('Error generating summary: ' + error.message);
    } finally {
        document.getElementById('summarize-btn').disabled = false;
        document.getElementById('summarize-btn').textContent = '✨ Generate Summary';
    }
}

let currentSummaryText = '';

function showSummaryResult(summary, mode, itemCount, wordCount) {
    currentSummaryText = summary;
    const modal = document.createElement('div');
    modal.className = 'summary-modal';
    modal.innerHTML = `
        <div class="summary-content">
            <h3>Generated Summary (${mode === 'ai' ? 'AI-Powered' : 'Simple Extractive'})</h3>
            <div class="summary-meta">
                <span>${itemCount} items • ${wordCount} words</span>
            </div>
            <div class="summary-text">${summary.replace(/\n/g, '<br>')}</div>
            <div class="summary-actions">
                <button class="btn btn-primary" id="copy-summary-btn">📋 Copy Summary</button>
                <button class="btn btn-primary" id="download-summary-btn">💾 Download</button>
                <button class="btn btn-danger" id="close-summary-btn">Close</button>
            </div>
        </div>
    `;
    document.body.appendChild(modal);

    document.getElementById('copy-summary-btn').addEventListener('click', () => {
        copySummary(currentSummaryText);
    });
    document.getElementById('download-summary-btn').addEventListener('click', () => {
        downloadSummary(currentSummaryText);
    });
    document.getElementById('close-summary-btn').addEventListener('click', closeSummaryModal);
}

function copySummary(text) {
    navigator.clipboard.writeText(text).then(() => {
        alert('Summary copied to clipboard!');
    }).catch(() => {
        alert('Failed to copy summary');
    });
}

async function downloadSummary(text) {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    const filename = `nt-qa-summary-${year}-${month}-${day}.txt`;

    try {
        // Check if running in Electron (has electronAPI)
        if (window.electronAPI && window.electronAPI.saveFile) {
            const result = await window.electronAPI.saveFile(text, filename);
            if (result.success) {
                await window.electronAPI.showNotification('Export Successful', `Summary saved as: ${result.filename}`);
            }
        } else {
            // Fallback for browser mode
            const blob = new Blob([text], { type: 'text/plain' });
            const url = URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            URL.revokeObjectURL(url);
        }
    } catch (error) {
        console.error('Error downloading summary:', error);
        alert('Error saving file');
    }
}

function closeSummaryModal() {
    const modal = document.querySelector('.summary-modal');
    if (modal) {
        modal.remove();
    }
}

function getSelectedItemsData() {
    return Array.from(selectedItems).map(id => {
        const [bookId, qaIndex] = id.split(':').map(Number);
        return { bookId, qaIndex };
    });
}

// ============= TAB SWITCHING =============
// Tab switching
document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        const tabName = btn.dataset.tab + '-tab';

        // Deactivate all tabs and buttons
        document.querySelectorAll('.tab-content').forEach(tab => tab.classList.remove('active'));
        document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));

        // Activate selected tab and button
        document.getElementById(tabName).classList.add('active');
        btn.classList.add('active');
    });
});

// Search functionality
document.getElementById('search-btn').addEventListener('click', async () => {
    const query = document.getElementById('search-input').value.trim();

    if (!query) {
        alert('Please enter a search term');
        return;
    }

    try {
        const response = await fetch(`/api/search?q=${encodeURIComponent(query)}`);
        const results = await response.json();

        const resultsContainer = document.getElementById('search-results');

        if (results.length === 0) {
            resultsContainer.innerHTML = '<div class="empty-state"><p>No results found</p></div>';
        } else {
            resultsContainer.innerHTML = results.map((result, index) => `
                <div class="qa-item">
                    <div class="qa-book">${result.book}</div>
                    <div class="qa-question">${result.question}</div>
                    <div class="qa-answer">${result.answer}</div>
                    <button class="btn-add-item" data-book-id="${result.bookId}" data-qa-index="${result.qaIndex}">➕ Add</button>
                </div>
            `).join('');
        }
    } catch (error) {
        console.error('Search error:', error);
        document.getElementById('search-results').innerHTML = '<div class="empty-state"><p>Error performing search</p></div>';
    }
});

// Allow Enter key to search
document.getElementById('search-input').addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        document.getElementById('search-btn').click();
    }
});

// Load books on page load
window.addEventListener('load', async () => {
    try {
        const response = await fetch('/api/books');
        const books = await response.json();

        const bookSelect = document.getElementById('book-select');
        books.forEach(book => {
            const option = document.createElement('option');
            option.value = book.id;
            option.textContent = book.name;
            bookSelect.appendChild(option);
        });
    } catch (error) {
        console.error('Error loading books:', error);
    }
});

// Browse books functionality
document.getElementById('book-select').addEventListener('change', async (e) => {
    const bookId = e.target.value;
    const container = document.getElementById('book-qa');
    const chapterSelect = document.getElementById('chapter-select');

    if (!bookId) {
        container.innerHTML = '';
        document.getElementById('chapter-selector').classList.add('hidden');
        return;
    }

    try {
        // Load chapters first
        await loadChapters(bookId);

        // Load all Q&A for the book
        const response = await fetch(`/api/book/${bookId}`);
        const book = await response.json();

        container.innerHTML = book.qa.map((qa, qaIndex) => `
            <div class="qa-item" data-chapter="${qa.chapter || 'N/A'}">
                <div class="qa-chapter">Chapter ${qa.chapter || 'Book-wide'}</div>
                <div class="qa-book">${book.name}</div>
                <div class="qa-question">${qa.q}</div>
                <div class="qa-answer">${qa.a}</div>
                <button class="btn-add-item" data-book-id="${book.id}" data-qa-index="${qaIndex}">➕ Add</button>
            </div>
        `).join('');

        updateSelectionUI();
    } catch (error) {
        console.error('Error loading book:', error);
        container.innerHTML = '<div class="empty-state"><p>Error loading book</p></div>';
    }
});

// Chapter selector change handler
document.getElementById('chapter-select').addEventListener('change', (e) => {
    const bookId = document.getElementById('book-select').value;
    const chapter = e.target.value === '' ? null : (e.target.value === 'null' ? null : e.target.value);

    if (bookId) {
        loadChapterQA(bookId, chapter);
    }
});

// Summary mode toggle (show/hide API key input)
document.querySelectorAll('input[name="summary-mode"]').forEach(radio => {
    radio.addEventListener('change', () => {
        const apiKeyInput = document.getElementById('openai-key');
        if (radio.value === 'ai' && radio.checked) {
            apiKeyInput.classList.remove('hidden');
        } else {
            apiKeyInput.classList.add('hidden');
        }
    });
});

// Summarize button click handler
document.getElementById('summarize-btn').addEventListener('click', generateSummary);

// Random question functionality
document.getElementById('random-btn').addEventListener('click', async () => {
    try {
        const response = await fetch('/api/random');
        const qa = await response.json();

        const container = document.getElementById('random-qa');
        container.innerHTML = `
            <div class="qa-item">
                <div class="qa-book">${qa.book}</div>
                <div class="qa-question">${qa.question}</div>
                <div class="qa-answer">${qa.answer}</div>
                <button class="btn-add-item" data-book-id="${qa.bookId}" data-qa-index="${qa.qaIndex}">➕ Add</button>
            </div>
        `;

        updateSelectionUI();
    } catch (error) {
        console.error('Error fetching random question:', error);
        document.getElementById('random-qa').innerHTML = '<div class="empty-state"><p>Error fetching question</p></div>';
    }
});

// ============= EVENT DELEGATION FOR ADD BUTTONS =============
document.addEventListener('click', (e) => {
    if (e.target.classList.contains('btn-add-item')) {
        const bookId = parseInt(e.target.dataset.bookId);
        const qaIndex = parseInt(e.target.dataset.qaIndex);
        toggleItemSelection(bookId, qaIndex);
    }
});

// ============= EXPORT FUNCTIONALITY =============
// Export all Q&A
document.getElementById('export-btn').addEventListener('click', async () => {
    try {
        const response = await fetch('/api/export');
        const textContent = await response.text();

        // Generate filename with date
        const today = new Date();
        const year = today.getFullYear();
        const month = String(today.getMonth() + 1).padStart(2, '0');
        const day = String(today.getDate()).padStart(2, '0');
        const filename = `nt-qa-export-${year}-${month}-${day}.txt`;

        // Check if running in Electron
        if (window.electronAPI && window.electronAPI.saveFile) {
            const result = await window.electronAPI.saveFile(textContent, filename);
            if (result.success) {
                await window.electronAPI.showNotification('Export Successful', `All Q&A exported to: ${result.filename}`);
            } else {
                alert('Export canceled');
            }
        } else {
            // Fallback for browser mode
            const blob = new Blob([textContent], { type: 'text/plain' });
            const url = URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            URL.revokeObjectURL(url);
            alert('Export completed! File saved as: ' + filename);
        }
    } catch (error) {
        console.error('Error exporting data:', error);
        alert('Error exporting data. Please try again.');
    }
});

// Export selected items
document.getElementById('export-selected-btn').addEventListener('click', async () => {
    if (selectedItems.size === 0) {
        alert('Please select at least one item to export');
        return;
    }

    try {
        const selected = getSelectedItemsData();

        const response = await fetch('/api/export-selected', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ selected })
        });

        const textContent = await response.text();

        // Generate filename with date
        const today = new Date();
        const year = today.getFullYear();
        const month = String(today.getMonth() + 1).padStart(2, '0');
        const day = String(today.getDate()).padStart(2, '0');
        const filename = `nt-qa-export-selected-${year}-${month}-${day}.txt`;

        // Check if running in Electron
        if (window.electronAPI && window.electronAPI.saveFile) {
            const result = await window.electronAPI.saveFile(textContent, filename);
            if (result.success) {
                await window.electronAPI.showNotification('Export Successful', `${selectedItems.size} items exported to: ${result.filename}`);
            } else {
                alert('Export canceled');
            }
        } else {
            // Fallback for browser mode
            const blob = new Blob([textContent], { type: 'text/plain' });
            const url = URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            URL.revokeObjectURL(url);
            alert(`Export completed! ${selectedItems.size} items saved as: ${filename}`);
        }
    } catch (error) {
        console.error('Error exporting selected data:', error);
        alert('Error exporting data. Please try again.');
    }
});

// ============= BANNER CONTROLS =============
document.getElementById('clear-selection-btn').addEventListener('click', () => {
    clearSelection();
});

document.getElementById('view-selection-btn').addEventListener('click', () => {
    const tabs = document.querySelectorAll('.tab-btn');
    tabs.forEach(tab => {
        if (tab.dataset.tab === 'export') {
            tab.click();
        }
    });
});
