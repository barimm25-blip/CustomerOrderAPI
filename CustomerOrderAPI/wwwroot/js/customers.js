const API = 'http://localhost:5237/api';
let editId = null;

async function loadCustomers() {
    const search = document.getElementById('searchInput')?.value ?? '';
    const url = search ? `${API}/customers?search=${search}` : `${API}/customers`;
    const customers = await fetch(url).then(r => r.json());
    const tbody = document.getElementById('customerTable');
    tbody.innerHTML = customers.map(c => `
        <tr>
            <td>${c.customerCode}</td>
            <td>${c.customerName}</td>
            <td>${c.contactName ?? '-'}</td>
            <td>${c.phone ?? '-'}</td>
            <td>${c.email ?? '-'}</td>
            <td>
                <button class="btn btn-secondary" onclick="openEditModal(${c.customerId})">Edit</button>
                <button class="btn btn-danger" onclick="deleteCustomer(${c.customerId})">Delete</button>
            </td>
        </tr>
    `).join('');
}

function openCreateModal() {
    editId = null;
    document.getElementById('modalTitle').textContent = 'Add Customer';
    document.getElementById('customerCode').value = '';
    document.getElementById('customerName').value = '';
    document.getElementById('contactName').value = '';
    document.getElementById('phone').value = '';
    document.getElementById('email').value = '';
    document.getElementById('createModal').style.display = 'flex';
}

async function openEditModal(id) {
    editId = id;
    const c = await fetch(`${API}/customers/${id}`).then(r => r.json());
    document.getElementById('modalTitle').textContent = 'Edit Customer';
    document.getElementById('customerCode').value = c.customerCode;
    document.getElementById('customerName').value = c.customerName;
    document.getElementById('contactName').value = c.contactName ?? '';
    document.getElementById('phone').value = c.phone ?? '';
    document.getElementById('email').value = c.email ?? '';
    document.getElementById('createModal').style.display = 'flex';
}

async function submitCustomer() {
    const dto = {
        customerCode: document.getElementById('customerCode').value,
        customerName: document.getElementById('customerName').value,
        contactName: document.getElementById('contactName').value,
        phone: document.getElementById('phone').value,
        email: document.getElementById('email').value
    };

    const url = editId ? `${API}/customers/${editId}` : `${API}/customers`;
    const method = editId ? 'PUT' : 'POST';

    const res = await fetch(url, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto)
    });

    if (res.ok) {
        closeModal();
        loadCustomers();
    } else {
        alert('Error saving customer');
    }
}

async function deleteCustomer(id) {
    if (!confirm('Are you sure to delete this customer?')) return;
    await fetch(`${API}/customers/${id}`, { method: 'DELETE' });
    loadCustomers();
}

function closeModal() {
    document.getElementById('createModal').style.display = 'none';
}

loadCustomers();
