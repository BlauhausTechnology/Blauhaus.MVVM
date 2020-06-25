const core = require('@actions/core');
var versionName = process.env.GITHUB_REF.replace('refs/heads/release/','');
core.exportVariable('VERSION_NAME', versionName);