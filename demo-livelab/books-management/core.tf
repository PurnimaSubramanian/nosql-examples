## This configuration was generated by terraform-provider-oci

resource oci_core_internet_gateway export_Internet-Gateway-vcn_nosql_demos {
  compartment_id = var.compartment_ocid
  display_name = "Internet Gateway-vcn_nosql_demos"
  enabled      = "true"
  vcn_id = oci_core_vcn.export_vcn_nosql_demos.id
}


resource oci_core_service_gateway export_Service-Gateway-vcn_nosql_demos {
  compartment_id = var.compartment_ocid
  display_name = "Service Gateway-vcn_nosql_demos"
  #route_table_id = <<Optional value not found in discovery>>
  services {
    service_id = data.oci_core_services.oci_services.services[0]["id"]
  }
  vcn_id = oci_core_vcn.export_vcn_nosql_demos.id
}

resource oci_core_subnet export_Public-Subnet-vcn_nosql_demos {
  #availability_domain = <<Optional value not found in discovery>>
  cidr_block     = "10.0.0.0/24"
  compartment_id = var.compartment_ocid
  dhcp_options_id = oci_core_vcn.export_vcn_nosql_demos.default_dhcp_options_id
  display_name    = "Public Subnet-vcn_nosql_demos"
  dns_label       = "sub04301224030"
  #ipv6cidr_block = <<Optional value not found in discovery>>
  prohibit_internet_ingress  = "false"
  prohibit_public_ip_on_vnic = "false"
  route_table_id             = oci_core_vcn.export_vcn_nosql_demos.default_route_table_id
  security_list_ids = [
    oci_core_vcn.export_vcn_nosql_demos.default_security_list_id,
  ]
  vcn_id = oci_core_vcn.export_vcn_nosql_demos.id
}

resource oci_core_subnet export_Private-Subnet-vcn_nosql_demos {
  #availability_domain = <<Optional value not found in discovery>>
  cidr_block     = "10.0.1.0/24"
  compartment_id = var.compartment_ocid
  dhcp_options_id = oci_core_vcn.export_vcn_nosql_demos.default_dhcp_options_id
  display_name    = "Private Subnet-vcn_nosql_demos"
  dns_label       = "sub04301224031"
  #ipv6cidr_block = <<Optional value not found in discovery>>
  prohibit_internet_ingress  = "true"
  prohibit_public_ip_on_vnic = "true"
  route_table_id             = oci_core_route_table.export_Route-Table-for-Private-Subnet-vcn_nosql_demos.id
  security_list_ids = [
    oci_core_security_list.export_Security-List-for-Private-Subnet-vcn_nosql_demos.id,
  ]
  vcn_id = oci_core_vcn.export_vcn_nosql_demos.id
}


resource oci_core_vcn export_vcn_nosql_demos {
  #cidr_block = <<Optional value not found in discovery>>
  cidr_blocks = [
    "10.0.0.0/16",
  ]
  compartment_id = var.compartment_ocid
  display_name = "vcn_nosql_demos"
  dns_label    = "vcnnosqldemos"
  #is_ipv6enabled = <<Optional value not found in discovery>>
}

resource oci_core_default_dhcp_options export_Default-DHCP-Options-for-vcn_nosql_demos {
  compartment_id = var.compartment_ocid
  display_name     = "Default DHCP Options for vcn_nosql_demos"
  domain_name_type = "CUSTOM_DOMAIN"
  manage_default_resource_id = oci_core_vcn.export_vcn_nosql_demos.default_dhcp_options_id
  options {
    custom_dns_servers = [
    ]
    #search_domain_names = <<Optional value not found in discovery>>
    server_type = "VcnLocalPlusInternet"
    type        = "DomainNameServer"
  }
  options {
    #custom_dns_servers = <<Optional value not found in discovery>>
    search_domain_names = [
      "vcnnosqldemos.oraclevcn.com",
    ]
    #server_type = <<Optional value not found in discovery>>
    type = "SearchDomain"
  }
}

resource oci_core_nat_gateway export_NAT-Gateway-vcn_nosql_demos {
  block_traffic  = "false"
  compartment_id = var.compartment_ocid
  display_name = "NAT Gateway-vcn_nosql_demos"
  vcn_id       = oci_core_vcn.export_vcn_nosql_demos.id
}

resource oci_core_route_table export_Route-Table-for-Private-Subnet-vcn_nosql_demos {
  compartment_id = var.compartment_ocid
  display_name = "Route Table for Private Subnet-vcn_nosql_demos"
  route_rules {
    #description = <<Optional value not found in discovery>>
    destination       = "0.0.0.0/0"
    destination_type  = "CIDR_BLOCK"
    network_entity_id = oci_core_nat_gateway.export_NAT-Gateway-vcn_nosql_demos.id
  }
  route_rules {
    #description = <<Optional value not found in discovery>>
    destination       = data.oci_core_services.oci_services.services[0]["cidr_block"]
    destination_type  = "SERVICE_CIDR_BLOCK"
    network_entity_id = oci_core_service_gateway.export_Service-Gateway-vcn_nosql_demos.id
  }
  vcn_id = oci_core_vcn.export_vcn_nosql_demos.id
}

resource oci_core_default_route_table export_Default-Route-Table-for-vcn_nosql_demos {
  compartment_id = var.compartment_ocid
  display_name = "Default Route Table for vcn_nosql_demos"
  manage_default_resource_id = oci_core_vcn.export_vcn_nosql_demos.default_route_table_id
  route_rules {
    #description = <<Optional value not found in discovery>>
    destination       = "0.0.0.0/0"
    destination_type  = "CIDR_BLOCK"
    network_entity_id = oci_core_internet_gateway.export_Internet-Gateway-vcn_nosql_demos.id
  }
}

resource oci_core_security_list export_Security-List-for-Private-Subnet-vcn_nosql_demos {
  compartment_id = var.compartment_ocid
  display_name = "Security List for Private Subnet-vcn_nosql_demos"
  egress_security_rules {
    #description = <<Optional value not found in discovery>>
    destination      = "0.0.0.0/0"
    destination_type = "CIDR_BLOCK"
    #icmp_options = <<Optional value not found in discovery>>
    protocol  = "all"
    stateless = "false"
    #tcp_options = <<Optional value not found in discovery>>
    #udp_options = <<Optional value not found in discovery>>
  }
  ingress_security_rules {
    #description = <<Optional value not found in discovery>>
    #icmp_options = <<Optional value not found in discovery>>
    protocol    = "6"
    source      = "10.0.0.0/16"
    source_type = "CIDR_BLOCK"
    stateless   = "false"
    tcp_options {
      max = "22"
      min = "22"
      #source_port_range = <<Optional value not found in discovery>>
    }
    #udp_options = <<Optional value not found in discovery>>
  }
  ingress_security_rules {
    #description = <<Optional value not found in discovery>>
    icmp_options {
      code = "4"
      type = "3"
    }
    protocol    = "1"
    source      = "0.0.0.0/0"
    source_type = "CIDR_BLOCK"
    stateless   = "false"
    #tcp_options = <<Optional value not found in discovery>>
    #udp_options = <<Optional value not found in discovery>>
  }
  ingress_security_rules {
    #description = <<Optional value not found in discovery>>
    icmp_options {
      code = "-1"
      type = "3"
    }
    protocol    = "1"
    source      = "10.0.0.0/16"
    source_type = "CIDR_BLOCK"
    stateless   = "false"
    #tcp_options = <<Optional value not found in discovery>>
    #udp_options = <<Optional value not found in discovery>>
  }
  vcn_id = oci_core_vcn.export_vcn_nosql_demos.id
}

resource oci_core_default_security_list export_Default-Security-List-for-vcn_nosql_demos {
  compartment_id = var.compartment_ocid
  display_name = "Default Security List for vcn_nosql_demos"
  egress_security_rules {
    #description = <<Optional value not found in discovery>>
    destination      = "0.0.0.0/0"
    destination_type = "CIDR_BLOCK"
    #icmp_options = <<Optional value not found in discovery>>
    protocol  = "all"
    stateless = "false"
    #tcp_options = <<Optional value not found in discovery>>
    #udp_options = <<Optional value not found in discovery>>
  }
  ingress_security_rules {
    #description = <<Optional value not found in discovery>>
    #icmp_options = <<Optional value not found in discovery>>
    protocol    = "6"
    source      = "0.0.0.0/0"
    source_type = "CIDR_BLOCK"
    stateless   = "false"
    tcp_options {
      max = "22"
      min = "22"
      #source_port_range = <<Optional value not found in discovery>>
    }
    #udp_options = <<Optional value not found in discovery>>
  }
  ingress_security_rules {
    #description = <<Optional value not found in discovery>>
    icmp_options {
      code = "4"
      type = "3"
    }
    protocol    = "1"
    source      = "0.0.0.0/0"
    source_type = "CIDR_BLOCK"
    stateless   = "false"
    #tcp_options = <<Optional value not found in discovery>>
    #udp_options = <<Optional value not found in discovery>>
  }
  ingress_security_rules {
    #description = <<Optional value not found in discovery>>
    icmp_options {
      code = "-1"
      type = "3"
    }
    protocol    = "1"
    source      = "10.0.0.0/16"
    source_type = "CIDR_BLOCK"
    stateless   = "false"
    #tcp_options = <<Optional value not found in discovery>>
    #udp_options = <<Optional value not found in discovery>>
  }
  ingress_security_rules {
    description = "Temporary for test only"
    #icmp_options = <<Optional value not found in discovery>>
    protocol    = "all"
    source      = "10.0.0.0/24"
    source_type = "CIDR_BLOCK"
    stateless   = "false"
    #tcp_options = <<Optional value not found in discovery>>
    #udp_options = <<Optional value not found in discovery>>
  }
  ingress_security_rules {
    description = "Allow HTTPS traffic"
    #icmp_options = <<Optional value not found in discovery>>
    protocol    = "6"
    source      = "0.0.0.0/0"
    source_type = "CIDR_BLOCK"
    stateless   = "false"
    tcp_options {
      max = "443"
      min = "443"
      #source_port_range = <<Optional value not found in discovery>>
    }
    #udp_options = <<Optional value not found in discovery>>
  }
  ingress_security_rules {
    description = "Allow Jakarta NoSQL demo traffic"
    #icmp_options = <<Optional value not found in discovery>>
    protocol    = "6"
    source      = "0.0.0.0/0"
    source_type = "CIDR_BLOCK"
    stateless   = "false"
    tcp_options {
      max = "8080"
      min = "8080"
      #source_port_range = <<Optional value not found in discovery>>
    }
    #udp_options = <<Optional value not found in discovery>>
  }
  manage_default_resource_id = oci_core_vcn.export_vcn_nosql_demos.default_security_list_id
}

