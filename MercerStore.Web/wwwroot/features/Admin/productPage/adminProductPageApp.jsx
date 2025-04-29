import React, {lazy, Suspense} from 'react';
import {Provider} from 'react-redux';
import {createRoot} from 'react-dom/client';


const PageSizeSelectorComponent = lazy(() => import('./components/PageSizeSelectorComponent'));
const PaginationComponent = lazy(() => import('./components/PaginationComponent'));
const ProductListComponent = lazy(() => import('./components/ProductListComponent'));
const SortComponent = lazy(() => import('./components/SortComponent'));
const TotalProductsInfoComponent = lazy(() => import('./components/TotalProductsInfoComponent'));
const FilterComponent = lazy(() => import('./components/FilterComponent'));
const CategoriesComponent = lazy(() => import('./components/CategoriesComponent'));

export const adminProductPageApp = (store) => {
    const portals = [
        {id: 'admin-product-page-size-root', component: <PageSizeSelectorComponent/>},
        {id: 'admin-product-pagination-root', component: <PaginationComponent/>},
        {id: 'admin-product-sort-root', component: <SortComponent/>},
        {id: 'admin-total-products-info-root', component: <TotalProductsInfoComponent/>},
        {id: 'admin-filter-root', component: <FilterComponent/>},
        {id: 'admin-product-list-root', component: <ProductListComponent/>},
        {id: 'admin-product-categories-list-root', component: <CategoriesComponent/>},
    ];

    portals.forEach(({id, component}) => {
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