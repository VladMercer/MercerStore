import React from 'react';
import { createRoot } from 'react-dom/client';
import CartAppWithPortals from './CartAppWithPortals';

const mainContainer = document.getElementById('cart-app');
if (mainContainer) {
    const root = createRoot(mainContainer);
    root.render(<CartAppWithPortals />);
} 