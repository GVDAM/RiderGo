# RiderGo 🚴‍♂️

Este projeto é uma API desenvolvida em **.NET 8** com **PostgreSQL** e utiliza o **Pub/Sub Emulator do Google** para mensageria.  

## 🚀 Como rodar o projeto

Existem duas formas de rodar o projeto:

- [Opção 1 - Docker (recomendado)](#opção-1---docker-recomendado)  
- [Opção 2 - Visual Studio](#opção-2---visual-studio)  

---

## Opção 1 - Docker (recomendado)

### Pré-requisitos
- [Docker](https://www.docker.com/get-started) instalado

### Passos
1. Clonar o repositório
   ```bash
   git clone https://github.com/GVDAM/RiderGo.git
   ```
2. Entrar na pasta raiz (onde está a solução `RiderGo.sln`)
   ```bash
   cd RiderGo
   ```
3. Subir os containers
   ```bash
   docker-compose up --build
   ```
4. Assim que tudo estiver no ar, acessar o **Swagger** em:  
   👉 [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

### Banco de Dados
- **Host:** `localhost`  
- **Porta:** `5433`  
- **Usuário:** `postgres`  
- **Senha:** `1q2w3e4r@#$`  

---

## Opção 2 - Visual Studio

### Pré-requisitos
- [Visual Studio 2022](https://visualstudio.microsoft.com/)  
- SDK **.NET 8**  
- [Docker](https://www.docker.com/get-started)  

### Passos
1. Clonar o repositório
   ```bash
   git clone https://github.com/GVDAM/RiderGo.git
   ```
2. Criar o banco de dados em container Docker
   ```bash
   docker run --name RiderGo -e POSTGRES_PASSWORD=1q2w3e4r@#$ -p 5432:5432 -d postgres
   ```
3. Subir o emulador de mensageria PubSub do Google
   ```bash
   docker run --rm -it -p 8085:8085 gcr.io/google.com/cloudsdktool/cloud-sdk:latest gcloud beta emulators pubsub start --project=rider-go --host-port=0.0.0.0:8085
   ```
4. Abrir a solução `RiderGo.sln` no Visual Studio  
5. Definir o projeto **RiderGo.API** como **Startup Project**  
6. Rodar a aplicação pelo Visual Studio  

### Banco de Dados
- **Host:** `localhost`  
- **Porta:** `5432`  
- **Usuário:** `postgres`  
- **Senha:** `1q2w3e4r@#$`  
