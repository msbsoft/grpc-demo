#!/usr/bin/env python3
"""
PreToolUse hook to block access to sensitive files
Returns: allow, deny, or ask
"""
import sys
import os
import json
import re

# List of sensitive file patterns to block
SENSITIVE_PATTERNS = [
    r'\.env.*',           # Environment files
    r'.*\.key$',          # Private keys
    r'.*\.pem$',          # Certificates
    r'\.ssh/.*',          # SSH directory
    r'\.aws/.*',          # AWS credentials
    r'secrets/.*',        # Secrets directory
    r'credentials/.*',    # Credentials directory
    r'database\.yml$',    # Database config
    r'docker-compose.*\.yml$',  # Docker configs
]

def main():
    # Read the tool call information from environment variables
    tool_name = os.environ.get('CLAUDE_TOOL_NAME', '')
    file_path = os.environ.get('CLAUDE_TOOL_FILE_PATH', '')

    # Only check file access tools
    if tool_name not in ['Read', 'Write', 'Edit']:
        print('allow')
        return

    if not file_path:
        print('allow')
        return

    # Normalize path for checking
    normalized_path = file_path.replace('\\', '/').lower()

    # Check against sensitive patterns
    for pattern in SENSITIVE_PATTERNS:
        if re.search(pattern, normalized_path, re.IGNORECASE):
            print('deny')
            sys.stderr.write(f"Access denied to sensitive file: {file_path}\n")
            return

    # Allow access to non-sensitive files
    print('allow')

if __name__ == '__main__':
    main()