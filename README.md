# IdealWedding API

## Running the project and development

To properly work on this project you'll have to have these tools installed:

1. Visual Studio 2019 Community with full support for ASP.NET 7.0+ installed
2. Latest Microsoft SQL Server Express
3. (Optional) Latest Microsoft SQL Server Management Studio (or Azure Data Studio)

No need to restore the database, it will be created and seeded by the first request.

Contact team lead in order to get the current version of the appsettings.Development.json file.

Please note! Make sure your instance name is SQLEXPRESS or change the configuration string on appsettings.Development.json file.

## Crafting commits (Git Flow)

Always checkout a new feature branch from development when doing work. Never directly work on the development or master branches. 

You should use JIRA UI to checkout new branches (Go to your JIRA story -> Create Branch -> Choose `ideal-wedding-app` repo and keep the same name for the branch as auto generated).

All merges to the development branch must be performed via PR.

After wrapping up with your feature branch progress, and before submitting a PR make sure to:

  1. Run `npm run lint` and `npm run build:staging-app` and `npm run build:staging-website` commands and make sure nothing is failing. Fix any failures/warnings before you proceed.

  2. Squash all of your feature branch commits into one atomic commit. Doing the squash procedure makes sure we have an atomic commit per JIRA story which makes our main branches' history easy to read. To do this use the following flow:

      - `git rebase -i HEAD~3` where 3 is the number of non-merge commits you're trying to squash. You CANNOT squash merge commits, so make sure to merge the development branch after squashing, or - if you already have merge commits in your branch, only select the ones up to the last merge commit.

      - You see all commits to be manipulated with listed one after the other. You should change the `pick` keyword to the first commits to `r` (shorthand for `reword`) in order to update the resulting commit message, and for the rest of the commits you can use `f` (shorthand for `fixup`) which will squash those commits into the first one and discard their commit message. To enter the editing you can hit the `s` key anywhere on the selection. You DO NOT have to edit the commit messages at this point, only edit the first word of each row accordingly! 

      - Save changes by hitting `ESC` after making changes and then hitting `:wq` to save.

      - If you used `reword` on the first commit you will see the alter commit message step next. Here you need to update the commit message to match the `IW-{storyNumber} - Commit message` pattern (ex. `IW-21 - Implemented login flow.`). Try to be as verbose as needed to describe your changes.

      - Again, save changes by hitting `ESC` after making changes and then hitting `:wq` to save.

      - Run `git push origin +{branch_name}` (ex. `git push origin +IW-21_login_flow`) in order to force push your updates onto the global repository. DO NOT run `git pull` or do any sort of merges before this.

  3. Merge the development branch into your branch and resolve any conflicts that you might have directly on your feature branch. This is a good point to re-run `npm run lint` and `npm run build staging-app` and `npm run build staging-website` commands to make sure everything is still passing and fix any issues as a part of the merge commit.

Note 1: You can create a PR even after step 1. It's even recommended to do so as that gives you an overview of your progress and you can easily see the changes and any pending commits to merge back in.

Note 2: ALWAYS target the `development` branch. Only after you assign a reviewer does your open PR becomes ready for review.

Note 3: Do not rewrite history! Only manipulate squash operations on your feature branches.

Note 4: If something goes wrong and you have to do a hotfix, name branches in the following pattern: `IW-xxx-hotfix` where `xxx` is the story number.

When you first run the dev server with `npm start` you will get an error thrown by Chrome that your certificate is invalid. To circumvent this do the following: 

1. Click `Certificate (invalid)` from the URL bar
2. Click the `Details` tab
3. Click `Copy to File...`
4. `Next` -> `Next` -> select where to save the certificate -> `Finish`
5. hit `Windows + R` and type `inetcpl.cpl`
6. click `Content` tab
7. click `Certificates`
8. click `Trusted Root Certification Authorities` tab
9. Click `Import` button
10. Import the cert
11. Go to chrome and Enable `chrome://flags/#allow-insecure-localhost` flag.
11. Re-run `npm start`