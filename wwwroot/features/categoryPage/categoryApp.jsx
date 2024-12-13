import React, { Suspense, lazy } from 'react';
import { Provider } from 'react-redux';
import { createRoot } from 'react-dom/client';


const PageSizeSelectorComponent = lazy(() => import('./components/PageSizeSelectorComponent'));
const PaginationComponent = lazy(() => import('./components/PaginationComponent'));
const ProductListComponent = lazy(() => import('./components/ProductListComponent'));
const SortComponent = lazy(() => import('./components/SortComponent'));
const TotalProductsInfoComponent = lazy(() => import('./components/TotalProductsInfoComponent'));
const FilterComponent = lazy(() => import('./components/FilterComponent'));

export const categoryApp = (store) => {
    const portals = [
        { id: 'category-page-size-root', component: <PageSizeSelectorComponent /> },
        { id: 'category-pagination-root', component: <PaginationComponent /> },
        { id: 'category-sort-root', component: <SortComponent /> },
        { id: 'total-products-info-root', component: <TotalProductsInfoComponent /> },
        { id: 'filter-root', component: <FilterComponent /> },
        { id: 'category-product-list-root', component: <ProductListComponent /> },
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
};