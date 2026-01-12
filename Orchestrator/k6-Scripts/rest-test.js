import http from "k6/http";
import { check, sleep } from "k6";
import exec from "k6/execution"; // İterasyon bilgisini almak için
import { Trend } from "k6/metrics"; // Özel metrik oluşturmak için

// 1. ADIM: Özel bir Trend metriği tanımlıyoruz.
// Test sonunda bu isimle (real_duration) yeni bir istatistik göreceksiniz.
const realDuration = new Trend("real_duration");

export let options = {
  scenarios: {
    load_test: {
      executor: "shared-iterations",
      vus: __ENV.VUS ? parseInt(__ENV.VUS) : 50,
      iterations: 10005, // Toplam 10005 istek
      maxDuration: "5m",
    },
  },
};

export default function () {
  const payloadSize = __ENV.PAYLOAD_SIZE
    ? parseInt(__ENV.PAYLOAD_SIZE)
    : 1024;

  const url = `http://localhost:5000/test/rest`;

  const res = http.post(
    `${url}?payloadSize=${payloadSize}`,
    "",
    {
      headers: { "Content-Type": "application/json" },
    }
  );

  // 2. ADIM: Global iterasyon sayısını kontrol ediyoruz.
  // shared-iterations executor'ında 'iterationInTest' tüm VU'lar arasında benzersizdir (0, 1, 2...).
  // İlk 5 istek (0, 1, 2, 3, 4) bu if bloğuna girmez.
  if (exec.scenario.iterationInTest >= 5) {
    // Sadece 5. istek ve sonrasının süresini bu metriğe ekliyoruz.
    realDuration.add(res.timings.duration);
  }

  check(res, {
    "HTTP 200": (r) => r.status === 200,
  });

  sleep(0.1);
}