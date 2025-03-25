import requests
import sys
import os
import tempfile
from bs4 import BeautifulSoup

def download_from_ssstik(tiktok_url: str) -> str:
    session = requests.Session()
    headers = {
        "User-Agent": "Mozilla/5.0",
        "Content-Type": "application/x-www-form-urlencoded"
    }
    data = {
        "id": tiktok_url,
        "locale": "en"
    }

    # Надсилання POST-запиту
    response = session.post("https://ssstik.io/abc", headers=headers, data=data)
    soup = BeautifulSoup(response.text, "html.parser")

    # Отримання першого посилання для завантаження відео
    link_tag = soup.find("a")
    if not link_tag or "href" not in link_tag.attrs:
        print("ERROR: Не вдалося знайти посилання на відео", file=sys.stderr)
        sys.exit(1)

    video_url = link_tag["href"]
    video_data = session.get(video_url).content

    # Створення тимчасового файлу
    temp_dir = tempfile.gettempdir()
    file_path = os.path.join(temp_dir, "tiktok_video.mp4")
    with open(file_path, "wb") as f:
        f.write(video_data)

    return file_path

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python download_tiktok.py <tiktok_url>", file=sys.stderr)
        sys.exit(1)

    tiktok_url = sys.argv[1]
    try:
        file_path = download_from_ssstik(tiktok_url)
        print(file_path)  # stdout для .NET інтеграції
    except Exception as e:
        print(f"ERROR: {str(e)}", file=sys.stderr)
        sys.exit(1)