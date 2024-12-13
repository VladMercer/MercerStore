import React, { lazy, Suspense } from 'react';
import { Provider } from 'react-redux';
import { createRoot } from 'react-dom/client';

const CartHeaderComponent = lazy(() => import('./components/CartHeaderComponent'));
const CartPageComponent = lazy(() => import('./components/CartPageComponent'));
const CartOffcanvasComponent = lazy(() => import('./components/CartOffcanvasComponent'));
const AddToCartButton = lazy(() => import('./components/AddToCartButton'));

export const cartApp = (store) => {
    const portals = [
        { id: 'cart-header-root', component: <CartHeaderComponent /> },
        { id: 'cart-page-root', component: <CartPageComponent /> },
        { id: 'cart-offcanvas-root', component: <CartOffcanvasComponent /> },
    ];

    portals.forEach(({ id, component }) => {
        const rootElement = document.getElementById(id);
        if (rootElement) {
            const root = createRoot(rootElement);
            root.render(
                <Provider store={store}>
                    <Suspense fallback={<div>Loading...</div>}>
                        {component}
                    </Suspense>
                </Provider>
            );
        }
    });

    document.querySelectorAll('[id^="add-to-cart-root-"]').forEach((rootElement) => {
        const productId = rootElement.getAttribute('data-product-id');
        const root = createRoot(rootElement);
        root.render(
            <Provider store={store}>
                <Suspense fallback={<div>Loading...</div>}>
                    <AddToCartButton productId={productId} />
                </Suspense>
            </Provider>
        );
    });
};