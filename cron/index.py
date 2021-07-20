from git import Repo
from datetime import date

working_tree_dir = "/Users/arhanbusam/Dropbox/Arhan's Documents/Unity/RPG/"
file = "dailyGitActivity/activityTracker.txt"
repo = Repo(working_tree_dir)

def alter_file(file):
    today = date.today()
    with open(file, "a") as f:
        f.write(today.strftime("%B %d, %Y") + "\n")

def git_activities(repo):
    today = date.today() 
    repo.git.add(A=True)
    repo.git.commit('-m', "Daily Commit")
    repo.git.push('origin', 'HEAD:refs/for/master')

def commit_routine():
    alter_file(file)
    git_activities(repo)