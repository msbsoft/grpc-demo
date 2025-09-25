---
description: "A simple greeting command"
argument-hint: "<your message>"
---

Hello, $ARGUMENTS!

This is a simple demonstration command that shows how Claude Code slash commands work.

Available variables:
- `$ARGUMENTS`: The text passed after the command name
- `$PROJECT_ROOT`: The root directory of your project

This command simply greets you with whatever arguments you provide.

Try running: `/hello world` or `/hello Claude Code user`