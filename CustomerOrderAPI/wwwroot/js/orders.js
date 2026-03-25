const API = 'http://localhost:5237/api';
let products = [];

async function loadOrders() {
    const status = document.getElementById('statusFilter').value;
    const url = status ? `${API}/orders?status=${status}` : `${API}/orders`;
    const orders = await fetch(url).then(r => r.json());
    const tbody = document.getElementById('orderTable');
    tbody.innerHTML = orders.map(o => `
        <tr>
            <td>${o.orderNo}</td>
            <td>${o.customerName}</td>
            <td>${new Date(o.orderDate).toLocaleDateString('th-TH')}</td>
            <td>${o.totalAmount.toLocaleString('th-TH')} ฿</td>
            <td><span class="badge badge-${o.status.toLowerCase()}">${o.status}</span></td>
            <td>
                ${o.status === 'Pending' ? `<button class="btn btn-warning" onclick="updateStatus(${o.orderId}, 'Confirmed')">Confirm</button>` : ''}
                ${o.status === 'Pending' ? `<button class="btn btn-danger" onclick="cancelOrder(${o.orderId})">Cancel</button>` : ''}
            </td>
        </tr>
    `).join('');
}

async function openCreateModal() {
    const customers = await fetch(`${API}/customers`).then(r => r.json());
    products = await fetch(`${API}/products`).then(r => r.json());
    document.getElementById('customerId').innerHTML = customers.map(c =>
        `<option value="${c.customerId}">${c.customerName}</option>`
    ).join('');
    document.getElementById('orderItems').innerHTML = '';
    addItem();
    document.getElementById('createModal').style.display = 'flex';
}

function addItem() {
    const div = document.createElement('div');
    div.className = 'item-row';
    div.innerHTML = `
        <select class="product-select">${products.map(p => `<option value="${p.productId}">${p.productName} (${p.stockQty})</option>`).join('')}</select>
        <input type="number" class="qty-input" value="1" min="1" placeholder="จำนวน">
        <button class="btn btn-danger" onclick="this.parentElement.remove()">ลบ</button>
    `;
    document.getElementById('orderItems').appendChild(div);
}

async function submitOrder() {
    const items = [...document.querySelectorAll('.item-row')].map(row => ({
        productId: parseInt(row.querySelector('.product-select').value),
        qty: parseInt(row.querySelector('.qty-input').value)
    }));
    const dto = {
        customerId: parseInt(document.getElementById('customerId').value),
        requestedDate: document.getElementById('requestedDate').value || null,
        remark: document.getElementById('remark').value,
        orderItems: items
    };
    const res = await fetch(`${API}/orders`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto)
    });
    if (res.ok) {
        closeModal();
        loadOrders();
        alert('สร้าง Order สำเร็จ!');
    } else {
        const err = await res.json();
        alert('Error: ' + err.message);
    }
}

async function updateStatus(id, newStatus) {
    await fetch(`${API}/orders/${id}/status`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ newStatus, changedBy: 'admin' })
    });
    loadOrders();
}

async function cancelOrder(id) {
    if (!confirm('ยืนยันการยกเลิก Order?')) return;
    await fetch(`${API}/orders/${id}`, { method: 'DELETE' });
    loadOrders();
}

function closeModal() {
    document.getElementById('createModal').style.display = 'none';
}

loadOrders();

