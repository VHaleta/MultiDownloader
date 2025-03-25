import sys
import os
import yt_dlp
import json

def list_formats(url):
    ydl_opts = {
        'quiet': True,
        'no_warnings': True,
        'skip_download': True,
    }

    with yt_dlp.YoutubeDL(ydl_opts) as ydl:
        info = ydl.extract_info(url, download=False)
        formats = info.get('formats', [])
        result = []

        for f in formats:
            resolution = None
            width = f.get('width')
            height = f.get('height')
            if width and height:
                resolution = f"{width}x{height}"
            elif f.get('resolution'):
                resolution = f.get('resolution')

            result.append({
                'Extension': f.get('ext'),
                'FileSize': f.get('filesize') or f.get('filesize_approx'),
                'Resolution': resolution,
                'Width': width,
                'Height': height,
                'Protocol': f.get('protocol')
            })

        print(json.dumps(result, indent=2, ensure_ascii=False))

def download_tiktok_video(url, target_width=None, target_height=None):
    output_dir = os.path.abspath(os.path.dirname(__file__))
    output_template = os.path.join(output_dir, '%(title)s.%(ext)s')

    # Default to best format if no resolution is specified
    selected_format = 'mp4/bestvideo+bestaudio'

    if target_width and target_height:
        # Try to find a matching format
        ydl_probe_opts = {
            'quiet': True,
            'no_warnings': True,
            'skip_download': True,
        }

        with yt_dlp.YoutubeDL(ydl_probe_opts) as ydl:
            info = ydl.extract_info(url, download=False)
            formats = info.get('formats', [])
            for f in formats:
                if f.get('width') == target_width and f.get('height') == target_height:
                    selected_format = f.get('format_id')
                    break

    ydl_opts = {
        'outtmpl': output_template,
        'quiet': True,
        'no_warnings': True,
        'format': selected_format
    }

    with yt_dlp.YoutubeDL(ydl_opts) as ydl:
        info = ydl.extract_info(url, download=True)
        downloaded_file = ydl.prepare_filename(info)
        return downloaded_file

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage:\n"
              "  python download_tiktok.py <url>\n"
              "  python download_tiktok.py <url> --formats\n"
              "  python download_tiktok.py <url> --download <width> <height>", file=sys.stderr)
        sys.exit(1)

    tiktok_url = sys.argv[1]

    try:
        if len(sys.argv) == 3 and sys.argv[2] == "--formats":
            list_formats(tiktok_url)

        elif len(sys.argv) == 5 and sys.argv[2] == "--download":
            width = int(sys.argv[3])
            height = int(sys.argv[4])
            file_path = download_tiktok_video(tiktok_url, width, height)
            print(file_path)

        else:
            file_path = download_tiktok_video(tiktok_url)
            print(file_path)

    except Exception as e:
        print(f"Error: {str(e)}", file=sys.stderr)
        sys.exit(1)
