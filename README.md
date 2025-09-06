# 🚀 EnargyWay

This project was developed during my internship at **PLS Petroleum Company**.
The goal was to design and implement a **complete web-based management system** that connects every role in the company — from the CEO down to the warehouse manager, delivery agent, and end-users.

Each role has its own dedicated dashboard and permissions, ensuring that no one can access another role’s data or functionality.

---

## 👑 CEO / Administrator

* Full control over products: **add, edit, delete**.
* Manage employees: **add, edit, or remove employee records**.
* Monitor **daily and monthly sales** through an interactive dashboard.
* Identify the **Top 10 best-selling products**.
* Track **low-stock items** in real time.
* All data is updated **live** — every action is reflected instantly without reloading the page.

---

## 📦 Warehouse Manager

* Access to all products with the ability to update their details.
* Any changes are instantly synchronized with both the CEO’s and users’ views.
* Manage customer orders by updating their status:

  * **Preparing** → the user sees “Your order is being prepared”.
  * **With Delivery Agent** → the user sees “Your order is out for delivery”.
* Once an order is assigned, it is automatically pushed to the relevant delivery agent’s dashboard.

---

## 🚚 Delivery Agent

* Receives assigned orders in real time.
* Full visibility over order details.
* Marks an order as **Delivered** once completed.
* Delivery confirmation automatically updates the sales reports for the CEO (daily & monthly).

---

## 👤 Customer / End User

* Can browse all products **without login**.
* To place an order or add items to the cart → **login is required**.
* Can track the status of any order (past or current):

  * Preparing
  * Out for delivery
  * Delivered

---

## ⚡ Key Features

* **Role-based access control**: Each role has unique permissions and dedicated dashboards.
* **Real-time synchronization**: Updates propagate instantly across all user roles.
* **Interactive dashboards** for analytics and decision-making.
* **User-friendly interface**: Simple browsing for customers, with login only required for purchases.

---

## 🛠️ Tech Stack

* **Frontend**:

  * Blazor Server (.NET)
  * HTML5, CSS3, JavaScript
  * Bootstrap / TailwindCSS (for responsive design)

* **Backend**:

  * ASP.NET Core Web API
  * C# (Object-Oriented Programming principles)

* **Database**:

  * MySQL / SQL Server (for data persistence)
  * Entity Framework Core (ORM for database operations)

* **Real-Time Communication**:

  * SignalR (instant updates across users)

* **Version Control & Deployment**:

  * Git & GitHub

---

✨ This system ensures smooth coordination across the entire company workflow, providing transparency, efficiency, and real-time insights for management.
