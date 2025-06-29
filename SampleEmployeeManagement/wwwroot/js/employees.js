let departments = [];
let statuses = [];
let employees = [];
let filteredEmployees = [];
let currentFilters = {
    searchTerm: '',
    departments: [],
    statuses: []
};

// Initialize the page
document.addEventListener('DOMContentLoaded', async function () {
    await fetchInitialData();
    initializeFilters();
    renderEmployees();
});

async function fetchInitialData() {
    try {
        // Fetch departments from API
        const deptResponse = await fetch('/Employees/GetDepartments');
        departments = await deptResponse.json();

        // Fetch statuses from API
        const statusResponse = await fetch('/Employees/GetStatuses');
        statuses = await statusResponse.json();

        // Fetch all employees
        const empResponse = await fetch('/Employees/GetAll');
        employees = await empResponse.json();
        filteredEmployees = [...employees];
    } catch (error) {
        console.error('Error fetching initial data:', error);
        showNotification('Error loading data. Please refresh the page.', 'error');
    }
}

async function applyFilters() {
    try {
        const response = await fetch('/Employees/Filter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                SearchTerm: currentFilters.searchTerm,
                Departments: currentFilters.departments,
                Statuses: currentFilters.statuses
            })
        });

        if (!response.ok) throw new Error('Network response was not ok');

        filteredEmployees = await response.json();
        renderEmployees();
        updateActiveFiltersDisplay();
    } catch (error) {
        console.error('Error applying filters:', error);
        showNotification('Error applying filters', 'error');
    }
}

async function viewEmployee(id) {
    try {
        const response = await fetch(`/Employees/Details/${id}`);
        if (!response.ok) throw new Error('Network response was not ok');

        const employee = await response.json();
        showViewEmployeeModal(employee);
    } catch (error) {
        console.error('Error viewing employee:', error);
        showNotification('Error viewing employee details', 'error');
    }
}

async function editEmployee(id) {
    try {
        const response = await fetch(`/Employees/Details/${id}`);
        if (!response.ok) throw new Error('Network response was not ok');

        const employee = await response.json();
        showEditEmployeeModal(employee);
    } catch (error) {
        console.error('Error editing employee:', error);
        showNotification('Error loading employee for edit', 'error');
    }
}

async function deleteEmployee(id) {
    if (confirm('Are you sure you want to delete this employee?')) {
        try {
            const response = await fetch(`/Employees/Delete/${id}`, {
                method: 'POST'
            });

            if (!response.ok) throw new Error('Network response was not ok');

            const result = await response.json();
            if (result.success) {
                // Refresh the employee list
                await fetchInitialData();
                applyFilters();
                showNotification('Employee deleted successfully', 'success');
            } else {
                throw new Error(result.message);
            }
        } catch (error) {
            console.error('Error deleting employee:', error);
            showNotification('Error deleting employee', 'error');
        }
    }
}

// Handle form submission
document.getElementById('employee-form').addEventListener('submit', async function (e) {
    e.preventDefault();
    const formData = new FormData(e.target);

    const employeeData = {
        Id: formData.get('id') ? parseInt(formData.get('id')) : 0,
        FirstName: formData.get('firstName'),
        LastName: formData.get('lastName'),
        Email: formData.get('email'),
        Department: formData.get('department'),
        Position: formData.get('position'),
        Status: formData.get('status') || 'Active',
        ProfileImage: formData.get('profileImage') || 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=40&h=40&fit=crop&crop=face'
    };

    try {
        let response, url, method;

        if (currentModalMode === 'add') {
            url = '/Employees/Create';
            method = 'POST';
        } else {
            url = '/Employees/Edit/' + employeeData.Id;
            method = 'POST';
        }

        response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(employeeData)
        });

        if (!response.ok) throw new Error('Network response was not ok');

        const result = await response.json();
        if (result.success) {
            // Refresh the employee list
            await fetchInitialData();
            applyFilters();
            hideEmployeeModal();
            showNotification(
                currentModalMode === 'add' ? 'Employee added successfully' : 'Employee updated successfully',
                'success'
            );
        } else {
            throw new Error(result.message);
        }
    } catch (error) {
        console.error('Error saving employee:', error);
        showNotification('Error saving employee: ' + error.message, 'error');
    }
});
