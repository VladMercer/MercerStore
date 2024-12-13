import React, { Suspense, lazy } from 'react';
import { Provider } from 'react-redux';
import { createRoot } from 'react-dom/client';

const SortComponent = lazy(() => import('./components/SortComponent'));
const PageSizeSelectorComponent = lazy(() => import('./components/PageSizeSelectorComponent'));
const PaginationComponent = lazy(() => import('./components/PaginationComponent'));
const ResultListComponent = lazy(() => import('./components/ResultListComponent'));
const TotalProductsInfoComponent = lazy(() => import('./components/TotalProductsInfoComponent'));

export const searchPageApp = (store) => {
    const portals = [
        { id: 'search-sort-root', component: <SortComponent /> },
        { id: 'search-page-size-root', component: <PageSizeSelectorComponent /> },
        { id: 'search-pagination-root', component: <PaginationComponent /> },
        { id: 'search-result-list-root', component: <ResultListComponent /> },
        { id: 'search-total-products-info-root', component: <TotalProductsInfoComponent /> },
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