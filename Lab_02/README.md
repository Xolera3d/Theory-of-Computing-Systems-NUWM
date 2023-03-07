# Лабораторна робота №2

Build Dockerfile

```bash
cd read_txt\Print.Lab
docker build -t read_txt .
```

Run using  volumes

```bash
cd read_txt
docker run --rm -v  %cd%\read:/app/read print_txt
```
