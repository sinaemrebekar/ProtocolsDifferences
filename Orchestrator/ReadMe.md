# Protocols Load Testing with k6

Bu proje, **WebSocket**, **gRPC** ve **REST** protokollerinin performansını **k6** ile ölçmek için hazırlanmıştır. Her bir protokol için ayrı load test scriptleri mevcuttur.

---

## İçerik

- `grpc-test.js` → gRPC servis testi
- `rest-test.js` → REST API testi
- `ws-test.js` → WebSocket testi
- `Protos/` → gRPC protokol buffer dosyaları (test.proto)
- `README.md` → Proje açıklaması

---

## Gereksinimler

- Node.js veya k6 yüklü olmalı
- k6 v0.45+ önerilir
- gRPC testleri için `.proto` dosyaları
- Docker yüklü olmalı. 

## Kullanım
- Docker-compose.yml dosyasını compose ederek kullanabilirsiniz.
- k6 testleri için gerekli dosyalar ProjeYolu/Orchestrator/k6-Scripts dizininde bulunmaktadır.
- Bu dizine gidip cmd'yi açtıktan sonra aşağıdaki komutları çalıştırarak yük testi yapabilirsiniz.
- k6 run rest-test.js
- k6 run grpc-test.js
- k6 run ws-test.js

