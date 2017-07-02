# Load vsix tools
. "C:\Users\dahall\Documents\Visual Studio 2010\Projects\VsixTools2.ps1"
# Set the version number of 'MyPackage' and fix the zip issue for uploading to the gallery.
$vsixPath = "C:\Users\dahall\Documents\Visual Studio 2010\Projects\AeroWizard\AeroWizardTemplates\bin\Release\AeroWizardTemplates.vsix"
Vsix-SetVersion -VsixPath $vsixPath -Version "2.0.3"
Vsix-FixInvalidMultipleFiles -VsixPath $vsixPath
