# AEP ProjectHub

AEP ProjectHub is a desktop application built with **C#**, **.NET**, and **WPF** that automates construction packet creation and document management workflows for utility engineering projects.

The application was developed to reduce manual effort associated with assembling project documentation, organizing packet deliverables, and distributing completed project files. ProjectHub streamlines the packet creation process by intelligently classifying documents, extracting metadata, generating PDFs, and routing completed packages to the appropriate locations.

---

## Features

### Automated Packet Generation

Create project packets with a single click:

- Construction Packet
- Clearing Packet
- Stores Packet

ProjectHub automatically merges selected PDFs and generates finalized packet documents in the proper project folder structure.

---

### Intelligent Document Classification

Documents are automatically assigned to applicable packet types based on company naming standards.

Examples:

| Document Type | Construction | Clearing | Stores |
|--------------|-------------|----------|--------|
| 01 Documents | ✅ | ✅ | ✅ |
| 02 Documents | ✅ | ❌ | ❌ |
| 04 Documents | ✅ | ❌ | ❌ |
| 06 Documents | ✅ | ❌ | ✅ |
| C2 Documents | ❌ | ✅ | ❌ |
| C3 Documents | ❌ | ✅ | ❌ |

Users may override assignments during review.

---

### Packet Review Interface

Prior to generation, users can:

- Review packet contents
- Include or exclude documents by packet type
- Reorder documents
- Remove unwanted documents
- Validate project metadata

This provides full control over the final packet output.

---

### Metadata Extraction

ProjectHub extracts project information directly from Job Card PDFs, including:

- Work Request Number (WR)
- Job Number
- DCP Number

This reduces manual entry and improves consistency across generated deliverables.

---

### PDF Processing

Features include:

- PDF merging
- Packet assembly
- File ordering
- Document validation
- Automatic output generation

Built using modern .NET PDF processing libraries.

---

### Storms Integration

Completed packet folders can optionally be routed to district-specific Storms repositories.

Features:

- District validation
- Automatic folder creation
- Network path routing
- Copy distribution workflows

Example:

```text
Y:\2026
└── 0162
    └── 0162-00012345678-01
        ├── CC DCP123456 12345678 Construction Packet.pdf
        ├── WR12345678 Clearing Packet.pdf
        └── WR12345678 Stores Packet.pdf
```

---

### Standard Document Management

ProjectHub automatically loads company-standard reference documents from a centralized repository and includes them in the packet review process.

Benefits:

- Consistent packet content
- Reduced user error
- Simplified packet assembly

---

## Technology Stack

### Languages

- C#
- SQL
- XAML

### Frameworks

- .NET
- WPF

### Libraries

- iText7
- ObservableCollection
- LINQ

### Development Tools

- Visual Studio
- Git
- GitHub

---

## Architecture

ProjectHub follows a service-oriented architecture designed to separate UI logic from business processes.

Core Components:

```text
Views
├── Dashboard
├── Package Manager
└── Package Review

Services
├── PDF Merge Service
├── PDF Text Extraction
├── Job Card Parsing
└── Metadata Processing

Models
├── PackageDocument
└── Project Metadata
```

This approach improves maintainability and simplifies future feature additions.

---

## Workflow

```text
1. Add PDFs
       ↓
2. Detect Job Card
       ↓
3. Extract Metadata
       ↓
4. Review Documents
       ↓
5. Build Construction Packet
       ↓
6. Build Clearing Packet
       ↓
7. Build Stores Packet
       ↓
8. Route to Storms (Optional)
```

---

## Key Benefits

- Reduces manual packet assembly effort
- Standardizes project documentation workflows
- Improves document organization
- Eliminates repetitive administrative tasks
- Increases consistency across packet deliverables
- Supports utility engineering operations and distribution design teams

---

## Future Enhancements

Planned improvements include:

- Email template generation
- Additional workflow automation
- Enhanced metadata extraction
- Expanded packet classification rules
- Automated update deployment
- Reporting and analytics dashboard
- User settings and administration tools

---

## Author

**Gerardo Flores**

Computer Science Graduate | Software Developer

Built to support utility engineering workflows through automation, standardization, and process improvement.
