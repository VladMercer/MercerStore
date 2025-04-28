import React from 'react';
import {createRoot} from 'react-dom/client';
import ProductListApp from './ProductListApp';
import PaginationApp from './PaginationApp';
import PageSizeSelectorApp from './PageSizeSelectorApp';
import SortApp from './SortApp';
import {SearchProvider} from './SearchContext';
import {CartProvider} from '../../cart/CartContext';


const renderInRoot = (elementId, Component, withCartProvider = false) => {
    const rootElement = document.getElementById(elementId);
    if (rootElement) {
        const root = createRoot(rootElement);
        root.render(
            <SearchProvider>
                {withCartProvider ? (
                    <CartProvider>
                        <Component/>
                    </CartProvider>
                ) : (
                    <Component/>
                )}
            </SearchProvider>
        );
    }
};

renderInRoot('search-sort-root', SortApp);
renderInRoot('search-page-size-root', PageSizeSelectorApp);
renderInRoot('search-pagination-root', PaginationApp);
renderInRoot('search-product-list-root', ProductListApp, true);


