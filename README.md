# 📌 Git Workflow – Medical Appointment Project

## 🔹 Branch struktura

- `main` → stabilna verzija (DEMO-ready)
- `dev` → integraciona grana (aktivni razvoj)
- `feature/*` → rad po taskovima
- `fix/*` → bug fix
- `chore/*` → infra / config / CI

---

## 🧭 Pravilo rada

- ✅ Jedan task = jedna grana  
- ✅ Sve grane se prave iz `dev`  
- ✅ PR ide u `dev`  
- ✅ `main` se update-uje samo preko `dev -> main`

---

## 🛠 Kako da kreneš da radiš task

### 1️⃣ Update lokalnog repoa

```bash
git checkout dev
git pull origin dev
```

2️⃣ Napravi novu granu za svoj task
Backend
```
git checkout -b feature/be-appointments-crud
```
Frontend
```
git checkout -b feature/fe-filtering
```
CLI / Embedded
```
git checkout -b feature/cli-sync-doctors
```
3️⃣ Radi normalno

Dodaj izmene:
```
git add .
git commit -m "feat(be): add appointments CRUD"

Push:

git push -u origin feature/be-appointments-crud
```
4️⃣ Otvori Pull Request

Na GitHub-u:

- Base branch = dev

- Compare = tvoja feature grana

- Linkuj Trello/Jira task

- Prebaci task u IN REVIEW

5️⃣ Merge

- Minimum 1 approval

Nakon merge-a:

- Task ide u DONE

- Obriši svoju feature granu

🚀 Spajanje u main (radi Team Lead)

- Kad je dev stabilan:

- Otvori PR: dev -> main

- Minimum 2 approvals

Merge


GGWP

➡ To je nova stabilna verzija (demo-ready).
