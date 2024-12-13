import React from 'react';
import { Provider } from 'react-redux';
import store from './store';
import { createRoot } from 'react-dom/client';
import { cartApp } from './features/cart/cartApp';
import { categoryApp } from './features/categoryPage/categoryApp';
import { searchPageApp } from './features/searchPage/searchPageApp';
import { reviewsApp } from './features/reviews/reviewsApp';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const MainAppWithProvider = () => (
    <Provider store={store}>
        <ToastContainer />
    </Provider>
);

const mainContainer = document.getElementById('main-app');
if (mainContainer) {
    const root = createRoot(mainContainer);
    root.render(<MainAppWithProvider />);
}

cartApp(store);

if (document.getElementById('category-product-list-root')) {
    categoryApp(store);
}
if (document.getElementById('search-result-list-root')) {
    searchPageApp(store);
}
if (document.getElementById('product-reviews-root')) {
    reviewsApp(store);
}
