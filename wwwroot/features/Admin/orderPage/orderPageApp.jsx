import React, { Suspense, lazy } from 'react';
import { Provider } from 'react-redux';
import { createRoot } from 'react-dom/client';


const PageSizeSelectorComponent = lazy(() => import('./components/PageSizeSelectorComponent'));
const PaginationComponent = lazy(() => import('./components/PaginationComponent'));
const OrdersListComponent = lazy(() => import('./components/OrdersListComponent'));
const SortComponent = lazy(() => import('./components/SortComponent'));
const TotalOrdersInfoComponent = lazy(() => import('./components/TotalOrdersInfoComponent'));
const StatusFilterComponent = lazy(() => import('./components/StatusFilterComponent'));
const TimePeriodFilterComponent = lazy(() => import('./components/TimePeriodFilterComponent'));
const AdminSearchBarComponent = lazy(() => import('./components/AdminSearchBarComponent'));

export const orderPageApp = (store) => {
    const portals = [
        { id: 'order-page-size-root', component: <PageSizeSelectorComponent /> },
        { id: 'order-pagination-root', component: <PaginationComponent /> },
        { id: 'order-sort-root', component: <SortComponent /> },
        { id: 'order-total-orders-info-root', component: <TotalOrdersInfoComponent /> },
        { id: 'order-status-filter-root', component: <StatusFilterComponent /> },
        { id: 'order-time-period-filter-root', component: <TimePeriodFilterComponent /> },
        { id: 'order-list-root', component: <OrdersListComponent /> },
        { id: 'order-search-bar-root', component: <AdminSearchBarComponent /> },
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