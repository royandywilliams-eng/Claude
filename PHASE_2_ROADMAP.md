# Phase 2: Wizard Screens Implementation Roadmap

**Status**: Ready to Begin (Phase 1 Complete)
**Estimated Duration**: 1-2 days
**Target Completion**: March 6-7, 2026

## Overview

Phase 2 implements the 6 interactive wizard screens that users will see when they launch the application. These screens guide users through project configuration step-by-step with form controls for data entry.

## Architecture

Each screen inherits from `UserControl` and contains:
- Input controls (TextBox, ComboBox, CheckBox, RadioButton, etc.)
- Validation logic
- Data binding to ProjectConfiguration model
- Event handlers for user interactions

### Screen Hierarchy
```
WizardPanel
├── Screen1_ProjectType
├── Screen2_TechStack
├── Screen3_KeyFeatures
├── Screen4_UIUXRequirements
├── Screen5_PerformanceScalability
└── Screen6_DeploymentDevOps
```

## Screen Specifications

### Screen 1: Project Type & Scope (150 lines)
**Location**: `UI/Screens/Screen1_ProjectType.cs`

**Controls**:
```
Project Name:
[_____________ TextBox _____________]

Project Type:
○ Web Application
○ Desktop Application
○ Backend API
○ CLI Tool

Complexity Level:
[Complexity ▼] (Simple, Medium, Advanced, Enterprise)

Target Audience:
[_____________ TextBox _____________]
```

**Data Binding**:
- TextBox "projectNameInput" → ProjectConfiguration.ProjectName
- RadioButton selection → ProjectConfiguration.ProjectType
- ComboBox "complexityCombo" → ProjectConfiguration.ComplexityLevel
- TextBox "targetAudienceInput" → ProjectConfiguration.TargetAudience

**Validation**:
- ProjectName required (not empty)
- ProjectType must be selected
- Error display on screen

**Event Handlers**:
- TextBox.TextChanged → OnConfigurationChanged()
- RadioButton.CheckedChanged → OnConfigurationChanged()
- ComboBox.SelectedIndexChanged → OnConfigurationChanged()

---

### Screen 2: Technology Stack (200 lines)
**Location**: `UI/Screens/Screen2_TechStack.cs`

**Controls**:
```
Primary Language:
[JavaScript/TypeScript ▼] (auto-selects frameworks)

Selected Frameworks:
☑ React
☑ Next.js
☐ Express
☐ (more based on language)

Database:
[PostgreSQL ▼] (12 options from Constants)

Additional Libraries:
☑ Lodash
☐ Axios
☐ (checkboxes)
```

**Data Binding**:
- ComboBox "languageCombo" → ProjectConfiguration.PrimaryLanguage
- CheckedListBox "frameworksChecklist" → ProjectConfiguration.Frameworks[]
- ComboBox "databaseCombo" → ProjectConfiguration.Database
- CheckedListBox "librariesChecklist" → ProjectConfiguration.Libraries[]

**Key Features**:
- Language ComboBox populated from AppConstants.LANGUAGES
- Framework CheckedListBox populated from AppConstants.FRAMEWORKS_BY_LANGUAGE[selected language]
- Database ComboBox from AppConstants.DATABASES
- Frameworks list updates when language changes

**Validation**:
- At least one framework selected
- Database selection (can be "None")

---

### Screen 3: Key Features (180 lines)
**Location**: `UI/Screens/Screen3_KeyFeatures.cs`

**Controls**:
```
Select Features Your Project Needs:

☐ Authentication
☐ Authorization
☐ Database Integration
☐ REST API
☐ Real-time Updates
☐ File Upload/Download
☐ Payment Integration
☐ Email/Notifications
☐ Admin Dashboard
☐ Analytics/Logging
☐ Search Functionality
☐ User Management

[Quick Sets:]
[E-commerce] [Blog CMS] [API Backend]
```

**Data Binding**:
- CheckBox for each feature → ProjectConfiguration.Features[featureName]

**Key Features**:
- All 12 features from ProjectConfiguration.Features dictionary
- Quick set buttons select pre-defined combinations:
  - E-commerce: Auth, DB, REST API, Payments, Search, Admin Dashboard
  - Blog CMS: Auth, DB, REST API, Admin Dashboard, Search
  - API Backend: Auth, DB, REST API, Analytics, Caching

---

### Screen 4: UI/UX Requirements (150 lines)
**Location**: `UI/Screens/Screen4_UIUXRequirements.cs`

**Controls**:
```
Design Framework:
[Bootstrap ▼]

☑ Responsive Design
☐ Dark Mode Support

Accessibility Requirements:
[WCAG 2.1 Level AA ▼]

☑ Mobile Compatibility
```

**Data Binding**:
- ComboBox "designFrameworkCombo" → ProjectConfiguration.DesignFramework
- CheckBox "responsiveCheckbox" → ProjectConfiguration.ResponsiveDesign
- CheckBox "darkModeCheckbox" → ProjectConfiguration.DarkModeSupport
- ComboBox "accessibilityCombo" → ProjectConfiguration.AccessibilityRequirements
- CheckBox "mobileCompatibilityCheckbox" → ProjectConfiguration.MobileCompatibility

**Options**:
- Design frameworks from AppConstants.DESIGN_FRAMEWORKS
- Accessibility from AppConstants.ACCESSIBILITY_STANDARDS

---

### Screen 5: Performance & Scalability (140 lines)
**Location**: `UI/Screens/Screen5_PerformanceScalability.cs`

**Controls**:
```
Expected User Count:
[10,000 - 100,000 ▼]

☑ Real-time Data Needs

Caching Strategy:
[Redis ▼]

☑ CDN Usage

Database Optimization Notes:
[__________________ TextBox __________________]
```

**Data Binding**:
- ComboBox "userCountCombo" → ProjectConfiguration.ExpectedUsers
- CheckBox "realtimeCheckbox" → ProjectConfiguration.RealtimeDataNeeds
- ComboBox "cachingCombo" → ProjectConfiguration.CachingStrategy
- CheckBox "cdnCheckbox" → ProjectConfiguration.CDNUsage
- TextBox "optimizationNotes" → ProjectConfiguration.DatabaseOptimization

**Options**:
- User counts from AppConstants.EXPECTED_USER_COUNTS
- Caching from AppConstants.CACHING_STRATEGIES

---

### Screen 6: Deployment & DevOps (150 lines)
**Location**: `UI/Screens/Screen6_DeploymentDevOps.cs`

**Controls**:
```
Hosting Platform:
[AWS ▼]

CI/CD Pipeline:
[GitHub Actions ▼]

☑ Docker Containers
☑ Kubernetes Orchestration

Monitoring Tools:
[Sentry ▼]
```

**Data Binding**:
- ComboBox "hostingCombo" → ProjectConfiguration.HostingPlatform
- ComboBox "cicdCombo" → ProjectConfiguration.CIPipeline
- CheckBox "dockerCheckbox" → ProjectConfiguration.UseDocker
- CheckBox "kubernetesCheckbox" → ProjectConfiguration.UseKubernetes
- ComboBox "monitoringCombo" → ProjectConfiguration.MonitoringTools

**Options**:
- Hosting from AppConstants.HOSTING_PLATFORMS
- CI/CD from AppConstants.CI_CD_PIPELINES
- Monitoring from AppConstants.MONITORING_TOOLS

---

## Implementation Steps

### Step 1: Create Screens Folder
```bash
cd D:\ProjectSpecGUI\UI
mkdir Screens
```

### Step 2: Create Base Screen Class (Optional but recommended)
Create `UI/BaseScreen.cs` with common functionality:
- OnConfigurationChanged() method
- Common styling
- Error label

### Step 3: Implement Each Screen File
Start with Screen1 and work sequentially:
1. Screen1_ProjectType.cs
2. Screen2_TechStack.cs
3. Screen3_KeyFeatures.cs
4. Screen4_UIUXRequirements.cs
5. Screen5_PerformanceScalability.cs
6. Screen6_DeploymentDevOps.cs

### Step 4: Update WizardPanel.cs
Modify `LoadScreen()` method to instantiate actual screen controls:

```csharp
private void LoadScreen(int screenNumber)
{
    currentScreen = screenNumber;
    screenContainer.Controls.Clear();

    UserControl screen = screenNumber switch
    {
        0 => new Screen1_ProjectType(configuration),
        1 => new Screen2_TechStack(configuration),
        2 => new Screen3_KeyFeatures(configuration),
        3 => new Screen4_UIUXRequirements(configuration),
        4 => new Screen5_PerformanceScalability(configuration),
        5 => new Screen6_DeploymentDevOps(configuration),
        _ => null
    };

    if (screen != null)
    {
        screen.Dock = DockStyle.Fill;
        screenContainer.Controls.Add(screen);
    }

    UpdateUI();
}
```

## Data Binding Pattern

Each screen follows this pattern:

```csharp
public partial class ScreenX_Name : UserControl
{
    private ProjectConfiguration configuration;

    public event EventHandler ConfigurationChanged;

    public ScreenX_Name(ProjectConfiguration config)
    {
        InitializeComponent();
        this.configuration = config;
        LoadData();
    }

    private void InitializeComponent()
    {
        // Create all controls
    }

    private void LoadData()
    {
        // Populate dropdowns from Constants
        // Load current values from configuration
    }

    private void Control_Changed(object sender, EventArgs e)
    {
        // Update configuration
        configuration.PropertyName = controlValue;
        OnConfigurationChanged();
    }

    protected void OnConfigurationChanged()
    {
        ConfigurationChanged?.Invoke(this, EventArgs.Empty);
    }
}
```

## Testing Checklist

After implementing Phase 2:

- [ ] All 6 screens load without errors
- [ ] Navigation between screens works (Previous, Next, Skip)
- [ ] Data persists when switching screens
- [ ] ComboBoxes populate from Constants
- [ ] CheckBox selections persist
- [ ] RadioButton selections work properly
- [ ] Framework list updates with language change
- [ ] Error messages display correctly
- [ ] Preview panel updates with screen data
- [ ] Validation prevents invalid progressions
- [ ] All menus still function
- [ ] Status bar updates correctly

## Git Workflow

```bash
# Create branch for Phase 2
git checkout -b Phase-2-Screens

# Make commits per screen
git add UI/Screens/Screen1_ProjectType.cs
git commit -m "Phase 2.1: Implement Screen 1 - Project Type & Scope"

git add UI/Screens/Screen2_TechStack.cs
git commit -m "Phase 2.2: Implement Screen 2 - Technology Stack"

# ... continue for each screen ...

# Final commit
git commit -m "Phase 2: Complete all 6 wizard screens implementation"

# Merge to master
git checkout master
git merge Phase-2-Screens
```

## Success Criteria

✅ Phase 2 will be complete when:
- All 6 screens implemented with proper controls
- User can navigate through all 6 screens
- Configuration data updates and persists
- Preview panel shows updated data
- No validation errors when running
- All menu operations still work
- Git history shows clean commits

## Timeline

- **Start**: Immediately after Phase 1
- **Estimated Duration**: 1-2 days
- **Testing**: 2-4 hours
- **Target Completion**: March 6-7, 2026
- **Next Phase**: Phase 3 - Configuration Tabs (March 8-10)

---

**Phase 1 is COMPLETE. Phase 2 is ready to begin!**
