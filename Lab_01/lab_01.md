# Лабораторна робота №1

## Hello World! (an example of minimal Dockerization)

```text
Hello from Docker!
This message shows that your installation appears to be working correctly.

To generate this message, Docker took the following steps:

1. The Docker client contacted the Docker daemon.
2. The Docker daemon pulled the "hello-world" image from the Docker Hub.
  (amd64)
3. The Docker daemon created a new container from that image which runs the
  executable that produces the output you are currently reading.
4. The Docker daemon streamed that output to the Docker client, which sent it
  to your terminal.

To try something more ambitious, you can run an Ubuntu container with:
 $ docker run -it ubuntu bash

Share images, automate workflows, and more with a free Docker ID:
 https://hub.docker.com/

For more examples and ideas, visit:
 https://docs.docker.com/get-started/
```

Версія

```
REPOSITORY   TAG     IMAGE ID     CREATED     SIZE
hello-world latest feb5d9fea6a5 17 months ago 13.3kB
```

## ellerbrock/alpine-bash-git

```bash
docker run -it -d --name lab01 --entrypoint /bin/bash ellerbrock/alpine-bash-git
14aad210faf06a3829aa21f58e59cf3a9bc0fa85792db243834162fef3e2f76

docker  exec -it lab01 bash
bash-4.4$
```

```bash
bash-4.4$ git --version
git version 2.18.1
```

```bash
bash-4.4$ whoami
download
```

```bash
bash-4.4$ cd /
bash-4.4$ ls
bin    dev    etc    home   lib    media  mnt    proc   root   run    sbin   srv    sys    tmp    usr    var
```

```bash
bash-4.4$ cd /home/download/

bash-4.4$ git clone https://github.com/ripienaar/free-for-dev.git
Cloning into 'free-for-dev'...
remote: Enumerating objects: 11646, done.
remote: Counting objects: 100% (56/56), done.
remote: Compressing objects: 100% (33/33), done.
remote: Total 11646 (delta 30), reused 45 (delta 23), pack-reused 11590
Receiving objects: 100% (11646/11646), 6.46 MiB | 1.51 MiB/s, done.
Resolving deltas: 100% (5982/5982), done.

bash-4.4$ ls
free-for-dev

bash-4.4$ exit
exit

docker restart lab01
lab01

docker  exec -it lab01 bash
bash-4.4$ ls
free-for-dev
```
