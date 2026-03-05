using System;
using System.Collections.Generic;

namespace ProjectSpecGUI.Core
{
    /// <summary>
    /// Central location for all configuration constants and options
    /// </summary>
    public static class AppConstants
    {
        // === PROJECT TYPES ===
        public static readonly string[] PROJECT_TYPES = {
            "Web Application",
            "Desktop Application",
            "Backend API",
            "CLI Tool"
        };

        public static readonly Dictionary<string, string> PROJECT_TYPE_DESCRIPTIONS = new Dictionary<string, string>
        {
            { "Web Application", "Frontend web app (SPA, SSR, etc.)" },
            { "Desktop Application", "Windows/Mac/Linux desktop app" },
            { "Backend API", "REST API or Backend service" },
            { "CLI Tool", "Command-line application or script" }
        };

        // === COMPLEXITY LEVELS ===
        public static readonly string[] COMPLEXITY_LEVELS = {
            "Simple",
            "Medium",
            "Advanced",
            "Enterprise"
        };

        // === PRIMARY LANGUAGES ===
        public static readonly string[] LANGUAGES = {
            "JavaScript/TypeScript",
            "Python",
            "C#",
            "Java",
            "Go",
            "Rust",
            "PHP",
            "Ruby",
            "Swift",
            "Kotlin"
        };

        // === FRAMEWORKS BY LANGUAGE ===
        public static readonly Dictionary<string, string[]> FRAMEWORKS_BY_LANGUAGE = new Dictionary<string, string[]>
        {
            { "JavaScript/TypeScript", new[] { "React", "Vue.js", "Angular", "Next.js", "Svelte", "Express", "Nest.js" } },
            { "Python", new[] { "Django", "Flask", "FastAPI", "Pyramid", "Tornado", "PyQt5", "Tkinter" } },
            { "C#", new[] { "ASP.NET Core", "Blazor", "WPF", "WinForms", "Unity", "Entity Framework" } },
            { "Java", new[] { "Spring Boot", "Quarkus", "Micronaut", "Play Framework", "Struts", "JavaFX" } },
            { "Go", new[] { "Gin", "Echo", "Revel", "Buffalo", "Fiber" } },
            { "Rust", new[] { "Actix-web", "Tokio", "Rocket", "Axum", "Tauri" } },
            { "PHP", new[] { "Laravel", "Symfony", "WordPress", "Yii", "CodeIgniter" } },
            { "Ruby", new[] { "Rails", "Sinatra", "Hanami", "Dry-rb" } },
            { "Swift", new[] { "SwiftUI", "Cocoa", "Vapor", "Perfect" } },
            { "Kotlin", new[] { "Spring Boot", "Ktor", "Android" } }
        };

        // === DATABASES ===
        public static readonly string[] DATABASES = {
            "None",
            "PostgreSQL",
            "MySQL",
            "MongoDB",
            "Firebase",
            "SQLite",
            "SQL Server",
            "Oracle",
            "Redis",
            "DynamoDB",
            "Cassandra",
            "ElasticSearch"
        };

        // === DESIGN FRAMEWORKS ===
        public static readonly string[] DESIGN_FRAMEWORKS = {
            "Bootstrap",
            "Tailwind CSS",
            "Material Design",
            "Foundation",
            "Bulma",
            "Semantic UI",
            "Custom CSS",
            "CSS-in-JS"
        };

        // === HOSTING PLATFORMS ===
        public static readonly string[] HOSTING_PLATFORMS = {
            "AWS",
            "Azure",
            "Google Cloud",
            "Heroku",
            "DigitalOcean",
            "Linode",
            "Vercel",
            "Netlify",
            "Railway",
            "Render",
            "Self-hosted VPS"
        };

        // === CI/CD PIPELINES ===
        public static readonly string[] CI_CD_PIPELINES = {
            "GitHub Actions",
            "GitLab CI",
            "Jenkins",
            "CircleCI",
            "Travis CI",
            "Azure DevOps",
            "AWS CodePipeline",
            "None (Manual)"
        };

        // === MONITORING TOOLS ===
        public static readonly string[] MONITORING_TOOLS = {
            "CloudWatch",
            "New Relic",
            "Datadog",
            "Sentry",
            "Prometheus",
            "ELK Stack",
            "Grafana",
            "AppDynamics",
            "None"
        };

        // === ARCHITECTURE PATTERNS ===
        public static readonly string[] ARCHITECTURE_PATTERNS = {
            "Monolithic",
            "Microservices",
            "Serverless",
            "MVC",
            "MVVM",
            "Layered",
            "Event-Driven",
            "CQRS"
        };

        // === AUTHENTICATION METHODS ===
        public static readonly string[] AUTH_METHODS = {
            "JWT",
            "OAuth 2.0",
            "Session-based",
            "API Keys",
            "Basic Auth",
            "Kerberos",
            "SAML",
            "Multi-factor Auth"
        };

        // === STATE MANAGEMENT (Frontend) ===
        public static readonly string[] STATE_MANAGEMENT = {
            "Redux",
            "Context API",
            "MobX",
            "Vuex",
            "Pinia",
            "Recoil",
            "Zustand",
            "TanStack Query",
            "Local component state"
        };

        // === TEST FRAMEWORKS ===
        public static readonly string[] TEST_FRAMEWORKS = {
            "Jest",
            "Mocha",
            "Pytest",
            "xUnit",
            "JUnit",
            "RSpec",
            "Go testing",
            "Rust testing",
            "Cypress",
            "Selenium",
            "Playwright"
        };

        // === CACHING STRATEGIES ===
        public static readonly string[] CACHING_STRATEGIES = {
            "None",
            "Redis",
            "Memcached",
            "In-memory",
            "Browser cache",
            "CDN cache",
            "Database cache"
        };

        // === EXPECTED USER COUNTS ===
        public static readonly string[] EXPECTED_USER_COUNTS = {
            "< 100",
            "100 - 1,000",
            "1,000 - 10,000",
            "10,000 - 100,000",
            "100,000 - 1,000,000",
            "> 1,000,000"
        };

        // === LICENSES ===
        public static readonly string[] LICENSES = {
            "MIT",
            "Apache 2.0",
            "GPL 3.0",
            "BSD 3-Clause",
            "ISC",
            "LGPL 3.0",
            "Proprietary",
            "Unlicense"
        };

        // === ACCESSIBILITY STANDARDS ===
        public static readonly string[] ACCESSIBILITY_STANDARDS = {
            "None",
            "WCAG 2.0 Level A",
            "WCAG 2.0 Level AA",
            "WCAG 2.0 Level AAA",
            "WCAG 2.1 Level A",
            "WCAG 2.1 Level AA",
            "WCAG 2.1 Level AAA"
        };

        // === PAYMENT PROCESSORS ===
        public static readonly string[] PAYMENT_PROCESSORS = {
            "Stripe",
            "PayPal",
            "Square",
            "Adyen",
            "2Checkout",
            "Authorize.net",
            "Payment gateway (choose later)"
        };

        // === EMAIL SERVICES ===
        public static readonly string[] EMAIL_SERVICES = {
            "SendGrid",
            "Mailgun",
            "AWS SES",
            "Brevo (Sendinblue)",
            "Postmark",
            "SparkPost",
            "Gmail/SMTP"
        };

        // === API DESIGN PATTERNS ===
        public static readonly string[] API_PATTERNS = {
            "REST",
            "GraphQL",
            "gRPC",
            "WebSocket",
            "WebRTC",
            "SOAP"
        };

        // === TEMPLATE DEFINITIONS ===
        public static readonly Dictionary<string, ProjectConfiguration> TEMPLATES = new Dictionary<string, ProjectConfiguration>
        {
            {
                "Blog CMS", new ProjectConfiguration
                {
                    ProjectName = "Blog CMS",
                    ProjectType = "Web Application",
                    PrimaryLanguage = "JavaScript/TypeScript",
                    Frameworks = new List<string> { "React", "Next.js" },
                    Database = "PostgreSQL",
                    DesignFramework = "Tailwind CSS",
                    Features = new Dictionary<string, bool>
                    {
                        { "Authentication", true },
                        { "Authorization", true },
                        { "Database Integration", true },
                        { "REST API", true },
                        { "Admin Dashboard", true },
                        { "Search Functionality", true }
                    }
                }
            },
            {
                "E-commerce Store", new ProjectConfiguration
                {
                    ProjectName = "E-commerce Store",
                    ProjectType = "Web Application",
                    PrimaryLanguage = "JavaScript/TypeScript",
                    Frameworks = new List<string> { "React", "Express" },
                    Database = "MongoDB",
                    Features = new Dictionary<string, bool>
                    {
                        { "Authentication", true },
                        { "Database Integration", true },
                        { "REST API", true },
                        { "Payment Integration", true },
                        { "Admin Dashboard", true },
                        { "Search Functionality", true }
                    }
                }
            }
        };
    }
}
