import React from 'react';
import {createRoot} from 'react-dom/client';
import PageSizeSelectorApp from './PageSizeSelectorApp';
import PaginationApp from './PaginationApp';
import ProductListApp from './ProductListApp';
import SortApp from './SortApp';
import TotalProductsInfoApp from './TotalProductsInfoApp';
import FilterApp from './FilterApp';
import {ProductProvider} from './ProductContext';
import {CartProvider} from '../../cart/CartContext';


const renderInRoot = (elementId, Component, withCartProvider = false) => {
    const rootElement = document.getElementById(elementId);
    if (rootElement) {
        const root = createRoot(rootElement);
        root.render(
            <ProductProvider>
                {withCartProvider ? (
                    <CartProvider>
                        <Component/>
                    </CartProvider>
                ) : (
                    <Component/>
                )}
            </ProductProvider>
        );
    }
};

renderInRoot('category-page-size-root', PageSizeSelectorApp);
renderInRoot('category-pagination-root', PaginationApp);
renderInRoot('category-sort-root', SortApp);
renderInRoot('total-products-info-root', TotalProductsInfoApp);
renderInRoot('filter-root', FilterApp);
renderInRoot('category-product-list-root', ProductListApp, true);




