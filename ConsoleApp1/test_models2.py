import urllib.request
import json
import sys

api_key = "YOUR_API_KEY_HERE"

url = f"https://generativelanguage.googleapis.com/v1beta/models?key={api_key}"
req = urllib.request.Request(url)
response = urllib.request.urlopen(req)
models_data = json.loads(response.read().decode('utf-8'))
all_models = [m['name'].replace('models/', '') for m in models_data.get('models', []) if 'generateContent' in m.get('supportedGenerationMethods', [])]

print(f"Testing {len(all_models)} models...")

for model in all_models:
    url = f"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={api_key}"
    data = {"contents":[{"parts":[{"text":"Tell me about tej mahal"}]}]}
    req = urllib.request.Request(url, data=json.dumps(data).encode('utf-8'), headers={'Content-Type': 'application/json'})
    
    try:
        response = urllib.request.urlopen(req, timeout=5)
        result = json.loads(response.read().decode('utf-8'))
        print(f"SUCCESS: {model}")
        break # stop on first success
    except urllib.error.HTTPError as e:
        print(f"HTTPError: {model} returned {e.code}")
    except Exception as e:
        pass
