import { useSelector } from 'react-redux';

export const useUsers = () => {

    
    const users = useSelector((state) => state.userPage.users);
    const pageNumber = useSelector((state) => state.userPage.pageNumber);
    const pageSize = useSelector((state) => state.userPage.pageSize);
    const totalPages = useSelector((state) => state.userPage.totalPages);
    const sortOrder = useSelector((state) => state.userPage.sortOrder);
    const totalUsers = useSelector((state) => state.userPage.totalUsers);
    const isLoaded = useSelector((state) => state.userPage.isLoaded);
    const isPageReset = useSelector((state) => state.userPage.isPageReset);
    const timePeriodFilter = useSelector((state) => state.userPage.timePeriodFilter);
    const filter = useSelector((state) => state.userPage.filter);
    const query = useSelector((state) => state.userPage.query);

    return {
        query,
        users,
        pageNumber,
        pageSize,
        totalPages,
        sortOrder,
        totalUsers,
        isLoaded,
        isPageReset,
        timePeriodFilter,
        filter,
    };
};