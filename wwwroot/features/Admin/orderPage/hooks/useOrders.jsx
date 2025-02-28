import { useSelector } from 'react-redux';

export const useOrders = () => {

    
    const orders = useSelector((state) => state.orderPage.orders);
    const pageNumber = useSelector((state) => state.orderPage.pageNumber);
    const pageSize = useSelector((state) => state.orderPage.pageSize);
    const totalPages = useSelector((state) => state.orderPage.totalPages);
    const sortOrder = useSelector((state) => state.orderPage.sortOrder);
    const totalOrders = useSelector((state) => state.orderPage.totalOrders);
    const isLoaded = useSelector((state) => state.orderPage.isLoaded);
    const isPageReset = useSelector((state) => state.orderPage.isPageReset);
    const timePeriodFilter = useSelector((state) => state.orderPage.timePeriodFilter);
    const statusFilter = useSelector((state) => state.orderPage.statusFilter);
    const query = useSelector((state) => state.orderPage.query);

    return {
        orders,
        pageNumber,
        pageSize,
        totalPages,
        sortOrder,
        totalOrders,
        isLoaded,
        isPageReset,
        timePeriodFilter,
        statusFilter,
        query
    };
};