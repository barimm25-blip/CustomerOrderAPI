const API = 'http://localhost:5237/api';
let editId = null;

async function loadProducts() {
    const products = await fetch(`${API}/products`).then(r => r.json());
    const tbody = document.getElementById('productTable');
    tbody.innerHTML = products.map(p => `
    <tr>

        <td>${p.productCode}</td>
        <td>${p.productName}</td>
                <td>
            ${p.imageUrl
            ? `<img src="${p.imageUrl}" style="width:48px;height:48px;object-fit:contain;border-radius:6px;border:1px solid #eee;">`
            : `<div style="width:48px;height:48px;background:#f0f0f0;border-radius:6px;display:flex;align-items:center;justify-content:center;font-size:20px;">📦</div>`
        }
        </td>
        <td>${p.unit ?? '-'}</td>
        <td>${p.unitPrice.toLocaleString('en-US')} ฿</td>
        <td>${p.stockQty}</td>
        <td>${p.minStockQty}</td>
        <td>
            <span class="badge ${p.stockQty <= p.minStockQty ? 'badge-cancelled' : 'badge-processing'}">
                ${p.stockQty <= p.minStockQty ? 'Low Stock' : 'Normal'}
            </span>
        </td>
        <td>
            <button class="btn btn-secondary" onclick="openEditModal(${p.productId})">Edit</button>
        </td>
    </tr>
`).join('');
}

function openCreateModal() {
    editId = null;
    document.getElementById('modalTitle').textContent = 'Add Product';
    document.getElementById('productCode').value = '';
    document.getElementById('productName').value = '';
    document.getElementById('unit').value = '';
    document.getElementById('unitPrice').value = '';
    document.getElementById('stockQty').value = '';
    document.getElementById('minStockQty').value = '';
    document.getElementById('imageUrl').value = '';
    document.getElementById('imagePreview').style.display = 'none';
    document.getElementById('createModal').style.display = 'flex';
}

async function openEditModal(id) {
    editId = id;
    const p = await fetch(`${API}/products/${id}`).then(r => r.json());
    document.getElementById('modalTitle').textContent = 'Edit Product';
    document.getElementById('productCode').value = p.productCode;
    document.getElementById('productName').value = p.productName;
    document.getElementById('unit').value = p.unit ?? '';
    document.getElementById('unitPrice').value = p.unitPrice;
    document.getElementById('stockQty').value = p.stockQty;
    document.getElementById('minStockQty').value = p.minStockQty;
    document.getElementById('imageUrl').value = p.imageUrl ?? '';
    const preview = document.getElementById('imagePreview');
    const img = document.getElementById('previewImg');
    if (p.imageUrl) {
        img.src = p.imageUrl;
        preview.style.display = 'block';
    } else {
        preview.style.display = 'none';
    }

    document.getElementById('createModal').style.display = 'flex';
}

async function submitProduct() {
    const dto = {
        productCode: document.getElementById('productCode').value,
        productName: document.getElementById('productName').value,
        unit: document.getElementById('unit').value,
        unitPrice: parseFloat(document.getElementById('unitPrice').value),
        stockQty: parseInt(document.getElementById('stockQty').value),
        minStockQty: parseInt(document.getElementById('minStockQty').value),
          imageUrl: document.getElementById('imageUrl').value || null
    };

    const url = editId ? `${API}/products/${editId}` : `${API}/products`;
    const method = editId ? 'PUT' : 'POST';

    const res = await fetch(url, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto)
    });

    if (res.ok) {
        closeModal();
        loadProducts();
    } else {
        alert('Error saving product');
    }
}

function closeModal() {
    document.getElementById('createModal').style.display = 'none';
}

loadProducts();