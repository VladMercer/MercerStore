import { useSelector } from "react-redux";

export const useMetrics = () => {



    const sales = useSelector((state) => state.metricPage.sales);
    const reviews = useSelector((state) => state.metricPage.reviews);
    const invoices = useSelector((state) => state.metricPage.invoices);
    const users = useSelector((state) => state.metricPage.users);
    const suppliers = useSelector((state) => state.metricPage.suppliers);
    const isLoaded = useSelector((state) => state.metricPage.isLoaded);

    return {
        sales,
        reviews,
        invoices,
        users,
        suppliers,
        isLoaded
    };
};