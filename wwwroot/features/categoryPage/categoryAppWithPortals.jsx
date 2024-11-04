import React, { useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import store from '../../store'; 
/*import PageSizeSelectorComponent from './components/PageSizeSelectorComponent';
import PaginationComponent from './components/PaginationComponent';
import ProductListComponent from './components/ProductListComponent';
import SortComponent from './components/SortComponent';
import TotalProductsInfoComponent from './components/TotalProductsInfoComponent';*/
import FilterComponent from './components/FilterComponent';

const renderInRoot = (elementId, Component) => {
    const rootElement = document.getElementById(elementId);
    if (rootElement) {
        const root = createRoot(rootElement);
        root.render(
            <Provider store={store}>
                <Component />
            </Provider>
        );
    }
};

const CategoryAppWithPortals = () => {
    useEffect(() => {
      /*  renderInRoot('category-page-size-root', PageSizeSelectorComponent);
        renderInRoot('category-pagination-root', PaginationComponent);
        renderInRoot('category-sort-root', SortComponent);
        renderInRoot('total-products-info-root', TotalProductsInfoComponent);*/
        renderInRoot('filter-root', FilterComponent);
     /*   renderInRoot('category-product-list-root', ProductListComponent);*/
    }, []);

    return null;
};

export default CategoryAppWithPortals;