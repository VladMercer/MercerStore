import React from 'react';
import {Provider} from 'react-redux';
import store from './store';
import {createRoot} from 'react-dom/client';
import {cartApp} from './features/cart/cartApp';
import {categoryApp} from './features/categoryPage/categoryApp';
import {searchPageApp} from './features/searchPage/searchPageApp';
import {reviewsApp} from './features/reviews/reviewsApp';
import {ToastContainer} from 'react-toastify';
import {adminSearchPageApp} from './features/Admin/searchPage/adminSearchPageApp';
import {adminProductPageApp} from './features/Admin/productPage/adminProductPageApp';
import {orderPageApp} from './features/Admin/orderPage/orderPageApp';
import {userPageApp} from './features/Admin/userPage/userPageApp';
import {reviewPageApp} from './features/Admin/reviewPage/reviewPageApp';
import {supplierPageApp} from './features/Admin/supplierPage/supplierPageApp';
import {invoicePageApp} from './features/Admin/invoicePage/invoicePageApp';
import {metricPageApp} from './features/Admin/metricPage/metricPageApp';
import 'react-toastify/dist/ReactToastify.css';

const MainAppWithProvider = () => (
    <Provider store={store}>
        <ToastContainer/>
    </Provider>
);

const mainContainer = document.getElementById('main-app');
if (mainContainer) {
    const root = createRoot(mainContainer);
    root.render(<MainAppWithProvider/>);
}

cartApp(store);

if (document.getElementById('category-product-list-root')) {
    categoryApp(store);
}
if (document.getElementById('admin-product-list-root')) {
    adminProductPageApp(store);
}
if (document.getElementById('order-list-root')) {
    orderPageApp(store);
}
if (document.getElementById('user-list-root')) {
    userPageApp(store);
}
if (document.getElementById('review-list-root')) {
    reviewPageApp(store);
}
if (document.getElementById('supplier-list-root')) {
    supplierPageApp(store);
}
if (document.getElementById('invoice-list-root')) {
    invoicePageApp(store);
}
if (document.getElementById('sales-chart-root')) {
    metricPageApp(store);
}
if (document.getElementById('search-result-list-root')) {
    searchPageApp(store);
}
if (document.getElementById('admin-search-result-table-root')) {
    adminSearchPageApp(store);
}
if (document.getElementById('product-reviews-root')) {
    reviewsApp(store);
}
