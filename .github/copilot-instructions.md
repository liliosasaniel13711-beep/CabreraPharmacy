# Copilot Instructions

## Project Guidelines
- For WinForms dashboard work, prefer Designer-serializable native controls/layout, preserve existing control names, event handlers, authentication, data bindings, and real data sources; keep database logic out of InitializeComponent and avoid adding UI packages.
- For the Super Admin dashboard, treat the project's existing Admin form and its Designer implementation as the primary visual reference. Keep changes limited to Super Admin files unless a shared-control fix is essential, and preserve Super Admin-specific navigation, permissions, live data, and logout/session behavior.