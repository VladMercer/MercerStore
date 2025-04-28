import React, {useEffect} from 'react';
import {createRoot} from 'react-dom/client';
import CartPageApp from './CartPageApp';
import CartHeaderApp from './CartHeaderApp';
import CartOffcanvasApp from './CartOffcanvasApp';
import AddToCartButton from './AddToCartButton';
import {CartProvider} from './CartContext';

const CartAppWithPortals = () => {
    useEffect(() => {
        <CartProvider></CartProvider>
        // Рендер CartHeaderApp
        const cartHeaderRoot = document.getElementById('cart-header-root');
        if (cartHeaderRoot) {
            const root = createRoot(cartHeaderRoot);
            root.render(
                <CartHeaderApp/>
            );
        }

        // Рендер CartPageApp
        const cartPageRoot = document.getElementById('cart-page-root');
        if (cartPageRoot) {
            const root = createRoot(cartPageRoot);
            root.render(
                <CartPageApp/>
            );
        }

        // Рендер CartOffcanvasApp
        const cartOffcanvasRoot = document.getElementById('cart-offcanvas-root');
        if (cartOffcanvasRoot) {
            const root = createRoot(cartOffcanvasRoot);
            root.render(
                <CartOffcanvasApp/>
            );
        }

        // Рендер каждого AddToCartButton
        const cartAddToCartButtonRoots = document.querySelectorAll('[id^="add-to-cart-root-"]');
        cartAddToCartButtonRoots.forEach((rootElement) => {
            const productId = rootElement.getAttribute('data-product-id');
            const root = createRoot(rootElement);
            root.render(
                <AddToCartButton productId={productId}/>
            );
        });
    }, []);

    return null; // Ничего не рендерим напрямую
};

// Рендер CartAppWithPortals в главном контейнере
const mainContainer = document.getElementById('cart-app');
if (mainContainer) {
    const root = createRoot(mainContainer);
    root.render(<CartAppWithPortals/>);
} else {
    console.error("Container with ID 'cart-app' not found");
}