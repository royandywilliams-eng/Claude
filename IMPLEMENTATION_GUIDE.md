# ProjectSpec GUI - Windows 11 Implementation Guide

## Project Overview

**ProjectSpec GUI** is a professional Windows 11 application that helps users configure and specify new coding projects in detail, then send those specifications directly to Claude API for code generation.

**Status**: Phase 1 - Core Framework (In Progress)
**Created**: March 5, 2026
**Technology**: C# WinForms (.NET 6+)

## Completed Components (Phase 1 Foundation)

### 1. ✅ ProjectConfiguration.cs (Core Data Model)
**Location**: `D:\ProjectSpecGUI\ProjectConfiguration.cs`

This is the heart of the application - a class that holds all project configuration data:

**Key Properties**:
- Wizard data (project name, type, description, complexity, audience)
- Technology stack (language, frameworks, database, libraries)
- Features (authentication, payments, search, etc.)
- UI/UX requirements (design framework, responsiveness, dark mode)
- Performance settings (expected users, caching, CDN)
- Deployment options (hosting, CI/CD, containers)
- Advanced configuration (stored as Dictionary for flexibility)
- Project metadata (author, version, license, etc.)

**Key Methods**:
- `GenerateMarkdownSpecification()` - Creates human-readable spec
- `GenerateClaudePrompt()` - Creates a prompt to send to Claude
- `Validate()` - Validates configuration is complete

### 2. ✅ Core/Constants.cs (Configuration Options)
**Location**: `D:\ProjectSpecGUI\Core\Constants.cs`

Central repository of all available options:
- 10 project types with descriptions
- 10 programming languages
- 70+ frameworks mapped by language
- 12 database options
- 7 design frameworks
- 11 hosting platforms
- 8 CI/CD pipelines
- 8 monitoring tools
- 8 architecture patterns
- 8 authentication methods
- 8 state management solutions
- 11 testing frameworks
- 7 caching strategies
- 6 user count ranges
- 8 licenses
- 7 accessibility standards
- Pre-built templates (Blog CMS, E-commerce, etc.)

**Total Options**: 200+ configurable items

## Phase 1: Core UI Framework (Remaining Work)

### 3. TODO: Program.cs
**Purpose**: Application entry point

```csharp
static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.DefaultFont = new Font("Segoe UI", 9);
        Application.Run(new MainForm());
    }
}
```

### 4. TODO: MainForm.cs (Main Application Window)
**Purpose**: Main window with hybrid UI layout

**Features**:
- Size: 1400x900px (Windows 11 responsive)
- Split layout with 3 panels:
  - **Left Panel (25%)**: Wizard screens
  - **Right Panel (50%)**: Configuration tabs
  - **Bottom Panel (25%)**: Specification preview
- Menu bar with File, Edit, View, Tools, Help
- Status bar showing current step and validation status
- Dark/Light mode support

**Layout Structure**:
```
┌─ Menu Bar ───────────────────────────────────┐
├─────────────┬──────────────┬─────────────────┤
│   Wizard    │   Config     │   Tabs Content  │
│  (Screens)  │   Tabs       │                 │
│             │ (Tab List)   │                 │
├─────────────┴──────────────┴─────────────────┤
│     Specification Preview (Real-time)        │
├───────────────────────────────────────────────┤
│ Status Bar | Validation | Line Count         │
└───────────────────────────────────────────────┘
```

### 5. TODO: UI/WizardPanel.cs
**Purpose**: Left panel containing wizard screens

**Features**:
- Screen navigation buttons (Previous, Next, Skip)
- Progress indicator (Screen 1 of 6)
- Screen transition animation
- Current screen display area
- Validation error display

**Methods**:
- `ShowScreen(int screenNumber)`
- `Next()`, `Previous()`, `Skip()`
- `ValidateCurrentScreen()`
- `GetConfigurationData()`

### 6. TODO: UI/PreviewPanel.cs
**Purpose**: Real-time specification preview (bottom)

**Features**:
- Automatically updates as user configures
- Syntax highlighting for code/JSON
- Tabs: Markdown preview | JSON | Claude Prompt
- Copy to Clipboard button
- Export to File button
- Auto-refresh when data changes

### 7. TODO: UI/Screens/Screen1_ProjectType.cs through Screen6_Deployment.cs

**Screen 1: Project Type & Scope**
- Radio buttons: Web App / Desktop / API / CLI
- TextBox: Project name (required)
- TextBox: Description
- TextBox: Target audience
- ComboBox: Complexity level

**Screen 2: Technology Stack**
- ComboBox: Language (JavaScript, Python, C#, etc.)
- CheckedListBox: Frameworks (auto-populated based on language)
- ComboBox: Database
- CheckedListBox: Additional libraries

**Screen 3: Key Features**
- CheckedListBox: 12 common features
- Help text for each feature
- "Select Common Sets" buttons (e.g., "E-commerce", "Blog", "API")

**Screen 4: UI/UX Requirements**
- ComboBox: Design framework
- CheckBox: Responsive design
- CheckBox: Dark mode
- ComboBox: Accessibility standard
- CheckBox: Mobile compatibility

**Screen 5: Performance & Scalability**
- ComboBox: Expected user count
- CheckBox: Real-time data needs
- ComboBox: Caching strategy
- CheckBox: CDN usage
- TextBox: Database optimization notes

**Screen 6: Deployment & DevOps**
- ComboBox: Hosting platform
- ComboBox: CI/CD pipeline
- CheckBox: Docker
- CheckBox: Kubernetes
- ComboBox: Monitoring tools

## Phase 2-6 Overview

### Phase 2: Wizard Screens (All 6)
- Implement data binding between screens and ProjectConfiguration
- Add validation for each screen
- Screen transitions with data persistence
- "Quick Start" buttons for common patterns

### Phase 3: Configuration Tabs (9 tabs)
1. Project Details - Name, version, author, license
2. Architecture & Design - Patterns, API design, auth methods
3. Frontend Specifics - State management, routing, testing (conditional)
4. Backend Specifics - API versioning, rate limiting, caching (conditional)
5. Database Design - Schema, relationships, indexes (conditional)
6. Testing & Quality - Coverage targets, tools, security scanning
7. Dependencies & Integration - External APIs, payment, email services
8. Timeline & Constraints - Deadline, team size, budget, restrictions
9. Documentation & Comments - Notes, business logic, requirements

### Phase 4: Configuration Engine
- ProjectConfiguration class ✅ (Complete)
- ConfigurationValidator class (validates all inputs)
- Specification generation (markdown, JSON, Claude prompt)
- Data serialization/deserialization

### Phase 5: Claude API Integration
- ClaudeAPIClient class (send configs, receive responses)
- Settings form (API key storage)
- History tracking (previous configurations sent)
- Response display

### Phase 6: Polish & Testing
- Windows 11 theming (dark/light modes)
- Template library UI
- Error handling and logging
- Unit tests
- Build as standalone .exe

## Data Flow Diagram

```
User Input
    ↓
Wizard Screens & Tabs
    ↓
ProjectConfiguration (in-memory)
    ↓
Real-time Preview Panel
    ↓
GenerateMarkdownSpecification()
GenerateClaudePrompt()
    ↓
Export or Send to Claude
    ↓
Claude API Response
```

## Key Design Decisions

1. **Wizard + Tabs Hybrid**: Guides beginners while allowing advanced users to jump to specific areas
2. **Real-time Preview**: Users see exactly what will be sent before sending
3. **Configuration-based**: All options defined in Constants.cs for easy expansion
4. **Flexible Advanced Config**: Dictionary-based for unlimited custom fields
5. **Direct API Integration**: Send specifications directly to Claude for code generation
6. **WinForms**: Native Windows look, lightweight, no additional runtime needed

## Development Checklist

### Phase 1: Core Framework
- [ ] Program.cs created and entry point working
- [ ] MainForm.cs with split layout (3 panels)
- [ ] WizardPanel.cs with screen navigation
- [ ] PreviewPanel.cs with real-time updates
- [ ] Basic styling applied

### Phase 2: Wizard Screens
- [ ] All 6 screens implemented
- [ ] Navigation working between screens
- [ ] Data binding to ProjectConfiguration
- [ ] Validation for each screen

### Phase 3: Configuration Tabs
- [ ] All 9 tabs created
- [ ] Conditional tab visibility based on project type
- [ ] Data binding and validation
- [ ] Tab switching functionality

### Phase 4: Configuration Engine
- [ ] Validator class created
- [ ] Serialization methods working
- [ ] Specification generation accurate
- [ ] JSON export/import working

### Phase 5: API Integration
- [ ] Claude API client created
- [ ] API key settings dialog
- [ ] Send to Claude functionality
- [ ] Response display
- [ ] History tracking

### Phase 6: Polish
- [ ] Windows 11 theming applied
- [ ] Templates system working
- [ ] Error handling comprehensive
- [ ] Testing complete
- [ ] .exe compiled and tested

## File Size Estimates

| File | Lines | Purpose |
|------|-------|---------|
| ProjectConfiguration.cs | 250 | Data model |
| Constants.cs | 300 | Configuration options |
| Program.cs | 20 | Entry point |
| MainForm.cs | 400 | Main window |
| WizardPanel.cs | 300 | Wizard container |
| Screen1-6.cs (each) | 150-250 | Wizard screens |
| ConfigurationTabs.cs | 250 | Tab container |
| Tab1-9.cs (each) | 200-300 | Individual tabs |
| PreviewPanel.cs | 200 | Preview pane |
| ClaudeAPIClient.cs | 200 | API integration |
| Other support | 500 | Validators, helpers |
| **Total** | **~4,000** | Full application |

## Building & Deployment

### Requirements
- Visual Studio 2022 (Community edition free)
- .NET 6 SDK or later
- Newtonsoft.Json NuGet package
- RestSharp or HttpClient (built-in)

### Build Steps
1. Create new WinForms Application project
2. Add ProjectConfiguration.cs and Core/Constants.cs
3. Implement remaining files
4. Add NuGet dependencies
5. Build Release configuration
6. Publish as self-contained .exe

### Distribution
- Single .exe file (no dependencies needed)
- Standalone, no installation required
- ~10-15 MB with Windows 11 integration

## Next Steps

1. Create Program.cs with application entry point
2. Implement MainForm.cs with core layout
3. Implement WizardPanel.cs with screen container
4. Implement PreviewPanel.cs with real-time updates
5. Create all 6 wizard screens
6. Implement tab system (9 tabs)
7. Add API integration
8. Test and polish

## Notes for Development

- Use `ProjectConfiguration` as the single source of truth
- Bind all UI controls to ProjectConfiguration properties
- Update PreviewPanel automatically on any configuration change
- All options come from Constants.cs (avoid hardcoding)
- Implement validation at both screen and field level
- Use async/await for API calls to prevent UI blocking
- Save configuration to AppData for user recovery

---

**Total Estimated Lines of Code**: 4,000-5,000
**Development Time Estimate**: 2-3 weeks
**Estimated Delivery**: March 12-19, 2026
