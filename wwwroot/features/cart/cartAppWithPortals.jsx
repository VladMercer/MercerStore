import React, { useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import store from '../../store';
import CartHeaderComponent from './components/CartHeaderComponent';
import CartPageComponent from './components/CartPageComponent';
import CartOffcanvasComponent from './components/CartOffcanvasComponent';
import AddToCartButton from './components/AddToCartButton';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
const CartAppWithPortals = () => {
    useEffect(() => {
        const portals = [
            { id: 'cart-header-root', component: <CartHeaderComponent /> },
            { id: 'cart-page-root', component: <CartPageComponent /> },
            { id: 'cart-offcanvas-root', component: <CartOffcanvasComponent /> },
        ];

        portals.forEach(({ id, component }) => {
            const rootElement = document.getElementById(id);
            if (rootElement) {
                const root = createRoot(rootElement);
                root.render(<Provider store={store}>{component}
                   
                </Provider>);
            }
        });

        document.querySelectorAll('[id^="add-to-cart-root-"]').forEach((rootElement) => {
            const productId = rootElement.getAttribute('data-product-id');
            const root = createRoot(rootElement);
            root.render(<Provider store={store}><AddToCartButton productId={productId} />
               
            </Provider>);
        });
    }, []);

    return (
        <>
            <ToastContainer />
        </>
    );
};

export default CartAppWithPortals;