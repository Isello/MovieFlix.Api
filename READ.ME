# 🎬 MovieFlix Analytics: Ecossistema de Engenharia de Dados

Este projeto simula um ambiente real de análise de dados cinematográficos. Ele abrange desde a operação (registro e avaliação de filmes) até a geração de inteligência de negócios (Data Mart) através de um pipeline de dados automatizado e resiliente.

---

## 🏗️ Arquitetura do Sistema

O projeto foi construído seguindo os princípios de camadas de dados (Medallion Architecture simplificada), garantindo que o banco de dados operacional nunca seja sobrecarregado por consultas analíticas pesadas.

1. **Camada Operacional (OLTP)**
   - **Tecnologia:** SQLite.
   - **Função:** Armazena os dados brutos de cadastros e avaliações em tempo real. É a base de escrita da aplicação.

2. **Camada de Dados Brutos (Data Lake)**
   - **Tecnologia:** Arquivos CSV.
   - **Função:** Atua como um repositório de persistência fria e imutável. Os dados do SQLite são extraídos para arquivos .csv na pasta compartilhada /DataLake.

3. **Camada Analítica (Data Warehouse)**
   - **Tecnologia:** PostgreSQL.
   - **Função:** Recebe os dados dos CSVs via processo de ETL (Extract, Transform, Load) utilizando o comando COPY.

4. **Entrega de Insights (Data Mart)**
   - **Tecnologia:** SQL Views.
   - **Função:** Visões pré-processadas que respondem a perguntas de negócio, prontas para consumo por ferramentas de BI.

---

## 🛠️ Tecnologias Utilizadas

* **Framework:** .NET 9 (C#)
* **Proxy Reverso:** Nginx
* **Bancos de Dados:** SQLite (Operacional) e PostgreSQL (Analítico)
* **Containerização:** Docker e Docker-Compose
* **Bibliotecas:** Entity Framework Core, Npgsql (Bulk Copy), CsvHelper

---

## 🚀 Como Executar

Certifique-se de ter o Docker e o Docker-Compose instalados no seu ambiente.

1. Na raiz do projeto, execute o comando:
   docker-compose up --build -d

2. O sistema executará automaticamente a rotina de automação de dados no Startup:
   - Seed inicial do banco SQLite.
   - Geração/Atualização dos arquivos CSV no Data Lake.
   - Carga (ETL) para o PostgreSQL.
   - Criação das Views analíticas no Data Warehouse.

3. Acesse a documentação da API (Swagger) em:
   http://localhost/swagger

---

## 📊 Consultas Analíticas (Data Mart)

Você pode validar os resultados e os insights gerados diretamente no container do banco analítico com os seguintes comandos:

**Top 10 Filmes Mais Bem Avaliados:**
docker exec -it movieflix_dw psql -U admin -d movieflix_analytics -c "SELECT * FROM view_top_10_filmes;"

**Nota Média por Faixa Etária:**
docker exec -it movieflix_dw psql -U admin -d movieflix_analytics -c "SELECT * FROM view_media_etaria;"

**Volume de Avaliações por País:**
docker exec -it movieflix_dw psql -U admin -d movieflix_analytics -c "SELECT * FROM view_votos_pais;"

---

## 🌐 Configuração de Rede e Proxy Reverso

O Nginx atua como porta de entrada única (Porta 80), protegendo a API e realizando o encaminhamento de requisições. Esta arquitetura garante:
- **Segurança:** Oculta a porta real da aplicação (8080).
- **Padronização:** Centraliza o acesso via protocolo HTTP padrão.
- **Escalabilidade:** Facilita a implementação futura de Load Balancing.

---