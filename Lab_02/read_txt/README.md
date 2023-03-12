# Лабораторна робота №2

Build Dockerfile

```bash
cd ./Print.Lab
docker build --no-cache -t print_txt .
```

### Volumes

```bash
cd ./..
```

Using `-v` flag

```bash
docker run --rm -v  %cd%\read:/app/read:ro print_txt
```

Using `--mount` flag

```bash
docker run --rm --mount type=bind,source=%cd%\read,target=/app/read,readonly print_txt
```

### Відмінності між поведінкою `-v`і `--mount`[🔗](https://docs.docker.com/storage/bind-mounts/#differences-between--v-and---mount-behavior)

**-v**

- Створить відсутню папку на хості

- ~~При монтуванні, вміст з цільової папки контейнера буде скопійований у папку на хості~~ **При монтуванні, вміст цільової папки контейнера буде замінений на вміст папки хоста**

**--mount**

- ~~[Видасть помилку, якщо файл або папка буде відсутня на хості](https://docs.docker.com/storage/bind-mounts/#differences-between--v-and---mount-behavior:~:text=as%20a%20directory.-,If%20you%20use%20%2D%2Dmount,-to%20bind%2Dmount)~~ **Створить відсутню папку на хості**

- При монтуванні, вміст цільової папки контейнера буде замінений на вміст папки хоста

---

##### Команди використані при тестувані

**-v**

```bash
docker run --rm  -it -d --name test --entrypoint /bin/bash -v  %cd%\read:/app/read print_txt
```

**--mount**

```bash
docker run --rm  -it -d --name test --entrypoint /bin/bash --mount type=bind,source=%cd%\read,target=/app/read print_txt
```

```bash
docker  exec -it test bash

docker rm --force test
```
