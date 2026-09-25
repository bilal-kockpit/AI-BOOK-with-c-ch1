import urllib.request
import json
import sys

api_key = "YOUR_API_KEY_HERE"
models = ["gemini-3.6-flash", "gemini-3.1-pro-preview", "gemini-flash-latest", "gemini-2.5-flash-lite"]

for model in models:
    url = f"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={api_key}"
    data = {"contents":[{"parts":[{"text":"Tell me about tej mahal"}]}]}
    req = urllib.request.Request(url, data=json.dumps(data).encode('utf-8'), headers={'Content-Type': 'application/json'})
    
    try:
        print(f"Testing {model}...")
        response = urllib.request.urlopen(req, timeout=10)
        result = json.loads(response.read().decode('utf-8'))
        print(f"SUCCESS: {model} responded with {len(result.get('candidates', []))} candidates")
    except urllib.error.HTTPError as e:
        print(f"HTTPError: {model} returned {e.code} - {e.reason}")
    except Exception as e:
        print(f"Error: {model} failed with {str(e)}")
