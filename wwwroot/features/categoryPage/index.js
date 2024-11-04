import React from 'react';
import { createRoot } from 'react-dom/client';
import CategoryAppWithPortals from './categoryAppWithPortals';


const mainContainer = document.getElementById('category-app');
if (mainContainer) {
    const root = createRoot(mainContainer);
    root.render(<CategoryAppWithPortals />);
}