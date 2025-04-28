import React, {lazy, Suspense} from 'react';
import {Provider} from 'react-redux';
import {createRoot} from 'react-dom/client';


const InvoicesChartComponent = lazy(() => import('./components/InvoicesChartComponent'));
const ReviewsChartComponent = lazy(() => import('./components/ReviewsChartComponent'));
const SalesChartComponent = lazy(() => import('./components/SalesChartComponent'));
const UsersChartComponent = lazy(() => import('./components/UsersChartComponent'));


export const metricPageApp = (store) => {
    const portals = [
        {id: 'invoices-chart-root', component: <InvoicesChartComponent/>},
        {id: 'reviews-chart-root', component: <ReviewsChartComponent/>},
        {id: 'sales-chart-root', component: <SalesChartComponent/>},
        {id: 'users-chart-root', component: <UsersChartComponent/>},
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