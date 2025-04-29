import React, {lazy, Suspense} from 'react';
import {Provider} from 'react-redux';
import {createRoot} from 'react-dom/client';


const PageSizeSelectorComponent = lazy(() => import('./components/PageSizeSelectorComponent'));
const PaginationComponent = lazy(() => import('./components/PaginationComponent'));
const InvoicesListComponent = lazy(() => import('./components/InvoicesListComponent'));
const SortComponent = lazy(() => import('./components/SortComponent'));
const TotalInvoicesInfoComponent = lazy(() => import('./components/TotalInvoicesInfoComponent'));
const FilterComponent = lazy(() => import('./components/FilterComponent'));
const TimePeriodFilterComponent = lazy(() => import('./components/TimePeriodFilterComponent'));
const AdminSearchBarComponent = lazy(() => import('./components/AdminSearchBarComponent'));

export const invoicePageApp = (store) => {
    const portals = [
        {id: 'invoice-page-size-root', component: <PageSizeSelectorComponent/>},
        {id: 'invoice-pagination-root', component: <PaginationComponent/>},
        {id: 'invoice-sort-root', component: <SortComponent/>},
        {id: 'invoice-total-info-root', component: <TotalInvoicesInfoComponent/>},
        {id: 'invoice-filter-root', component: <FilterComponent/>},
        {id: 'invoice-time-period-filter-root', component: <TimePeriodFilterComponent/>},
        {id: 'invoice-list-root', component: <InvoicesListComponent/>},
        {id: 'invoice-search-bar-root', component: <AdminSearchBarComponent/>},
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