Как запустить Redis:

docker compose -f docker-compose.redis.yml up -d


### Для работы голосового ввода необходимо:

## 1. Создать два ресурса в Azure:
- Speech service -для распознавания речи
- Translator (Cognitive Services) —для перевода текста

После создания вы получите:
- API-ключи
- Регион
- Endpoint для Translator (Text Translation Endpoint)

## 2. Создать файл .env в корне проекта Agora.WebApi
Добавьте туда переменные:

AZURE_SPEECH_KEY=your-speech-api-key
AZURE_SPEECH_REGION=your-region (например, francecentral)
AZURE_TRANSLATOR_KEY=your-translator-key
AZURE_TRANSLATOR_ENDPOINT=https://api.cognitive.microsofttranslator.com