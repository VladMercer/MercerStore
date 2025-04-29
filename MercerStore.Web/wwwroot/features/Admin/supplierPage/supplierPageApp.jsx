import React, {lazy, Suspense} from 'react';
import {Provider} from 'react-redux';
import {createRoot} from 'react-dom/client';


const PageSizeSelectorComponent = lazy(() => import('./components/PageSizeSelectorComponent'));
const PaginationComponent = lazy(() => import('./components/PaginationComponent'));
const SuppliersListComponent = lazy(() => import('./components/SuppliersListComponent'));
const TotalSuppliersInfoComponent = lazy(() => import('./components/TotalSuppliersInfoComponent'));
const AdminSearchBarComponent = lazy(() => import('./components/AdminSearchBarComponent'));

export const supplierPageApp = (store) => {
    const portals = [
        {id: 'supplier-page-size-root', component: <PageSizeSelectorComponent/>},
        {id: 'supplier-pagination-root', component: <PaginationComponent/>},
        {id: 'supplier-total-info-root', component: <TotalSuppliersInfoComponent/>},
        {id: 'supplier-list-root', component: <SuppliersListComponent/>},
        {id: 'supplier-search-bar-root', component: <AdminSearchBarComponent/>},
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