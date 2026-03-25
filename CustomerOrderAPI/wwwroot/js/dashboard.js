const API = 'http://localhost:5237/api';

async function loadDashboard() {
    const summary = await fetch(`${API}/dashboard/summary`).then(r => r.json());
    document.getElementById('todayOrders').textContent = summary.todayOrders;
    document.getElementById('todayAmount').textContent = summary.todayAmount.toLocaleString('th-TH', { style: 'currency', currency: 'THB' });
    document.getElementById('pendingOrders').textContent = summary.pendingOrders;
    document.getElementById('lowStock').textContent = summary.lowStockCount;

    const byStatus = await fetch(`${API}/dashboard/orders-by-status`).then(r => r.json());
    new Chart(document.getElementById('statusChart'), {
        type: 'doughnut',
        data: {
            labels: byStatus.map(x => x.status),
            datasets: [{
                data: byStatus.map(x => x.count),
                backgroundColor: ['#f39c12', '#3498db', '#2ecc71', '#9b59b6', '#1abc9c', '#e74c3c']
            }]
        },
        options: { plugins: { legend: { position: 'bottom' } } }
    });

    const lowStock = await fetch(`${API}/dashboard/low-stock`).then(r => r.json());
    const list = document.getElementById('lowStockList');
    if (lowStock.length === 0) {
        list.innerHTML = '<p style="color:#888;font-size:14px">No items are running low</p>';
    } else {
        list.innerHTML = lowStock.map(p => `
            <div class="low-stock-item">
                <span>${p.productName}</span>
                <span class="stock-num">${p.stockQty} / ${p.minStockQty}</span>
            </div>
        `).join('');
    }
}

loadDashboard();