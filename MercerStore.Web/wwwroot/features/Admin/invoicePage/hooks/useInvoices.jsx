import { useSelector } from 'react-redux';

export const useInvoices = () => {

    
    const invoices = useSelector((state) => state.invoicePage.invoices);
    const pageNumber = useSelector((state) => state.invoicePage.pageNumber);
    const pageSize = useSelector((state) => state.invoicePage.pageSize);
    const totalPages = useSelector((state) => state.invoicePage.totalPages);
    const sortOrder = useSelector((state) => state.invoicePage.sortOrder);
    const totalInvoices = useSelector((state) => state.invoicePage.totalInvoices);
    const isLoaded = useSelector((state) => state.invoicePage.isLoaded);
    const isPageReset = useSelector((state) => state.invoicePage.isPageReset);
    const timePeriodFilter = useSelector((state) => state.invoicePage.timePeriodFilter);
    const filter = useSelector((state) => state.invoicePage.filter);
    const query = useSelector((state) => state.invoicePage.query);

    return {
        query,
        invoices,
        pageNumber,
        pageSize,
        totalPages,
        sortOrder,
        totalInvoices,
        isLoaded,
        isPageReset,
        timePeriodFilter,
        filter,
    };
};