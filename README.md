# CDS.security:



### Log In Platform

A full-stack web application for logging in and managing users with role-based
access control.

**Technologies**
- Backend: C# / ASP.NET Core (.NET 10) REST API, documented with Swagger (NSwag)
- Database: PostgreSQL, accessed through linq2db
- Frontend: React 19 with TypeScript, React Router and Bun
- Authentication: JWT bearer tokens, valid for 30 minutes

**Architecture**
- client: the React frontend
- server/api: controllers and application setup
- service: business logic, permission checks and token generation
- server/be: entities, DTOs and the role enum
- server/db: database connection, table mappings and initialization

**Features**
- Log in with a username and password, and log out
- Passwords are stored as hashes (ASP.NET Core PasswordHasher)
- View, create, edit and delete users, based on the logged-in user's role
- Confirmation popup before a user is deleted
- The user is logged out automatically when the token is expired or invalid

**Roles**
- Admin: sees all users and can manage all of them
- SuperUser: sees and manages only regular users, and cannot promote anyone
- User: sees regular users, but cannot manage anyone

Permissions are enforced on the server. The frontend also hides the buttons that
the logged-in user's role is not allowed to use.


**Password Hashing**

Passwords are never saved in plain text. When a user is created, the register
method in UserService passes the raw password to the ASP.NET Core PasswordHasher.
It generates a random salt and hashes the password with PBKDF2, returning a single
string that contains both the salt and the hash. Only this string is saved, in the
password_hash column.

<img width="533" height="66" alt="image" src="https://github.com/user-attachments/assets/11fc0e94-90c0-4b10-86dd-059ef86d618b" />

A hash cannot be turned back into the password, so at login the password is
verified instead of decrypted. The login method finds the user by username and
calls VerifyHashedPassword, which hashes the entered password with the stored salt
and compares the results. If they do not match, the login is rejected.

<img width="743" height="260" alt="image" src="https://github.com/user-attachments/assets/c575ee41-a879-496f-ac49-670e4c1edbeb" />

**JWT Tokens**

After a successful login, the server creates a JWT token. It contains the user's
id, username and role as claims, expires after 30 minutes and is signed with a
secret key using HMAC-SHA256. The signature makes it possible to detect if the
token has been changed, for example if someone edits their own role.

<img width="649" height="300" alt="image" src="https://github.com/user-attachments/assets/c42f77bb-ed03-4cd5-87f7-6eca47376eae" />

The client saves the token in localStorage and sends it in the Authorization header
of its requests as "Bearer <token>". It also decodes the token to display the
username and show the buttons that match the role. If the server responds with 401,
the token is removed and the user is logged out.

<img width="556" height="398" alt="image" src="https://github.com/user-attachments/assets/4871c463-3b63-4e6c-86a1-4c6854b983ca" />

On the server, the JWT middleware validates the signature, issuer, audience and
expiry time of the token on each request to a protected endpoint. Requests without
a valid token are rejected with 401.

<img width="617" height="264" alt="image" src="https://github.com/user-attachments/assets/ac5c2246-6481-4f7c-9794-dea924b15eb9" />

For a valid token, the controller reads the role from the token's claims and passes
it to UserService, which checks whether the user may perform the action. If not, the
server responds with 403.

<img width="498" height="397" alt="image" src="https://github.com/user-attachments/assets/dc450ea5-4e60-489b-89cb-fc26aaee13d9" />




# CDS.infrastructure:

**2.1 The Company's Network**

Kvist & Bjælke ApS requires a structured network infrastructure that allows employees, servers, and other devices to communicate efficiently.

The proposed network will be divided into several logical networks using VLANs. Each VLAN will serve a specific category of devices.

The following VLANs will be used:

VLAN ID | Network Name | Devices | Network Address
570 | Clients | 15 desktop computers and 10 laptops | 10.5.70.0/24
571 | Servers | 3 servers hosting internal systems and applications | 10.5.71.0/24
572 | Printers | 3 network printers | 10.5.72.0/28
573 | Voice | 12 IP phones | 10.5.73.0/27
574 | Surveillance | 6 surveillance cameras | 10.5.74.0/28
575 | Guests | Smartphones and computers belonging to visitors | 10.5.75.0/24
576 | Management | Switches, routers, and 4 wireless access points | 10.5.76.0/27

**Network Segmentation**

The network is divided into separate VLANs to improve security, organization, and network management.

- Client VLAN (570): Desktop computers and laptops are connected to the same network because they are used by employees for everyday office tasks.
- Server VLAN (571): Servers are separated from employee computers to protect internal systems and applications.
- Printer VLAN (572): Printers are placed in a separate network to make printer management easier and separate them from employee devices.
- Voice VLAN (573): IP phones have their own network to separate voice traffic from regular data traffic.
- Surveillance VLAN (574): Surveillance cameras are isolated from other devices to improve security.
- Guest VLAN (575): Guest devices are placed in a separate network to prevent visitors from accessing the company's internal resources.
- Management VLAN (576): Network equipment, including switches, routers, and wireless access points, is placed in a separate network for centralized management.

This segmentation allows the company to manage different types of devices independently and control communication between networks.

**IP Address Allocation**

Each VLAN will have its own default gateway, using the first available IP address in its subnet.

<img width="296" height="163" alt="image" src="https://github.com/user-attachments/assets/76c7653c-75bf-4b58-b074-f4a1ca5ff38d" />


**Static IP Addresses and DHCP**

Both static IP addressing and DHCP will be used in the company's network.

Static IP addresses will be assigned to devices that require predictable IP addresses:

- Servers.
- Network printers.
- Surveillance cameras.
- Network equipment, including switches, routers, and wireless access points.

DHCP will automatically assign IP addresses to devices that frequently connect and disconnect from the network:

- Desktop computers.
- Laptops.
- IP phones.
- Guest devices.

This approach simplifies network administration while ensuring that important infrastructure devices always have predictable IP addresses.

**2.1.2 Subnetting**

Subnetting allows a network to be divided into smaller logical networks. Each subnet has a network address, a subnet mask, and a specific number of available host addresses.

The following two networks have been selected to demonstrate subnetting.

Network 1 – Client VLAN (570)

- Network address: 10.5.70.0
- Subnet mask: 255.255.255.0
- CIDR notation: /24
- First available host: 10.5.70.1
- Last available host: 10.5.70.254
- Broadcast address: 10.5.70.255
- Available host addresses: 254

The client network contains 15 desktop computers and 10 laptops, resulting in 25 devices.

A /24 subnet provides 254 available host addresses, which is more than sufficient for the current number of devices.

It also provides enough capacity for future expansion if the company hires additional employees or introduces more computers.

Network 2 – Printer VLAN (572)

- Network address: 10.5.72.0
- Subnet mask: 255.255.255.240
- CIDR notation: /28
- First available host: 10.5.72.1
- Last available host: 10.5.72.14
- Broadcast address: 10.5.72.15
- Available host addresses: 14

The printer network contains 3 network printers.

A /28 subnet provides 14 available host addresses, which is sufficient for the current number of printers.

It also leaves 11 additional host addresses available for future expansion.

**Conclusion**

The proposed IP addressing plan provides a structured network infrastructure for Kvist & Bjælke ApS.

By dividing devices into separate VLANs and assigning appropriate IP address ranges, the company can improve network organization, security, and management.

The selected subnet sizes accommodate the current devices while providing room for future expansion.

**2.2 Establishing a Server on VLAN 571**

To deploy the login platform developed in CDS.security on Kvist & Bjælke ApS's server infrastructure, a virtual machine (chushpan-Appv2) was created in Proxmox (node pve02) with the following specifications:

- Operating System: Ubuntu Server
- Network: VLAN 571 (Server VLAN - 10.5.71.0/24)
- Assigned Server IP: 10.5.71.229
- Virtualization: Proxmox Virtual Machine
- Hosting: Docker container
- Selected TCP Port: 8000 (selected within the permitted 8000–8100 range)

After installing Ubuntu Server on the VM, Docker was installed to containerize and run the login platform (C# API, PostgreSQL database, and Bun + React frontend). Port 8000 was mapped from the container to the host VM, making the web application accessible across the network at http://10.5.71.229:8000.


**2.3 Establishing a Client on VLAN 570**

To allow employees to access the login platform from a different network segment than the server, an additional virtual machine (VM 112: chushpan-App-Client) was created in Proxmox to function as a client machine.

The two virtual machines are separated across different VLANs:
- The Server VM is connected to VLAN 571 (10.5.71.0/24) with the IP address 10.5.71.229.
- The Client VM (VM 112) is connected to VLAN 570 (10.5.70.0/24).

After configuring the client machine's network settings on VLAN 570 and verifying its IP configuration, communication with the server on VLAN 571 was tested.
Using the Firefox browser inside the Client VM on VLAN 570, we accessed the login platform on the Server VM via http://10.5.71.229:8000. As shown in the screenshot below,
the application loads and functions properly from the client machine, showing the logged-in user ("romann") and the role-based user list.

<img width="1855" height="1041" alt="image" src="https://github.com/user-attachments/assets/70c3c5eb-5af3-4024-a04b-f336b6a7aea2" />




Final Reflection:
Technical challenges we encountered: Architecture understanting throughout the team members, not everybody was understanding the desired architecture, even though it was fixed after.
Things learnt when deploying an application on a server: Creating a vitrual machine with Ubuntu Server, installing all the needed tools like: git, docker, after which running the container.
If the solution was to be deployed in a real company on company's server we would additionally set up a firewall.


Contributors: Roman Yurchenko, Török Dávid, Artem Sapehin.
Github link: https://github.com/Roman63245475/log_in_platform/
