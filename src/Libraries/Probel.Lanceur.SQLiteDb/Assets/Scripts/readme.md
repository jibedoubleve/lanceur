# Automatic update of the database

## Instructions

If you want to create an update script for the database
 
 1. Create a file based on this pattern `update-x.x.sql` while `x.x` should be a valid version number higher than the highest existing version
 1. Save this file into `\src\Probel.Lanceur.SQLiteDb\Assets\Scripts\`
 1. Set the property as _Embedded Resource_